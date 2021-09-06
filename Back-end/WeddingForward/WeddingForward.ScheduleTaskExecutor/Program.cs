using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using WeddingForward.ApplicationServices;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.Data;
using WeddingForward.Data.Models;
using WeddingForward.ScheduleTaskExecutor.Scenarios;
using Task = System.Threading.Tasks.Task;

namespace WeddingForward.ScheduleTaskExecutor
{
    class Program
    {
        private static readonly IConfiguration Configuration;

        private static readonly IServiceProvider ServiceProvider;

        static Program()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"appSettings.json", true, true)
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<PostCheckerScenario>();
            serviceCollection.AddTransient<AccountsMonitoringScenario>();

            serviceCollection.AddLogging(builder => { builder.ClearProviders().AddConsole(); });

            string connection = Configuration.GetConnectionString("DefaultConnection");

            serviceCollection.AddOptions();
            serviceCollection.Configure<SystemAccountsCollection>(Configuration.GetSection(nameof(SystemAccountsCollection)));

            serviceCollection.AddDbContext<WeddingForwardContext>(options => options.UseSqlServer(connection));
            serviceCollection.UseApplicationServices();
            ServiceProvider = serviceCollection.BuildServiceProvider();

            Utils.ApplicationLogging.Factory = ServiceProvider.GetService<ILoggerFactory>();
        }

        static async Task Main(string[] args)
        {
            WeddingForwardContext db = ServiceProvider.GetService<WeddingForwardContext>();

            ScriptsScheduleSet schedule = await db.ScriptsSchedule.Where(set => !set.IsStarted)
                .OrderBy(set => set.PlanedStart).FirstOrDefaultAsync()
                .ConfigureAwait(false);

            try
            {
                if (schedule == null || schedule.PlanedStart.ToUniversalTime() > DateTime.UtcNow)
                {
                    return;
                }

                schedule.IsStarted = true;

                db.ScriptsSchedule.Update(schedule);

                await db.SaveChangesAsync().ConfigureAwait(false);

                string[] @params = schedule.Args?.Split(';');
                string scriptName = schedule.ScriptType;

                IScenario currentScenario = null;

                switch (scriptName)
                {
                    case "PostChecker":
                    {
                        currentScenario = ServiceProvider.GetService<PostCheckerScenario>();

                        break;
                    }
                    case "AccountsMonitoring":
                    {
                        currentScenario = ServiceProvider.GetService<AccountsMonitoringScenario>();

                        db.ScriptsSchedule.Add(new ScriptsScheduleSet
                        {
                            Id = Guid.NewGuid(),
                            IsStarted = false,
                            IsFinished = false,
                            ScriptType = scriptName,
                            Args = null,
                            PlanedStart = DateTime.UtcNow.AddHours(1)
                        });

                        await db.SaveChangesAsync().ConfigureAwait(false);

                        break;
                    }
                }

                if (currentScenario != null)
                {
                    await currentScenario.RunAsync(@params).ConfigureAwait(false);
                }

                schedule.IsFinished = true;
                db.ScriptsSchedule.Update(schedule);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await Utils.ApplicationLogging.LogException(JsonConvert.SerializeObject(e), db).ConfigureAwait(false);
            }
            finally
            {
                ScheduleTask(DateTime.UtcNow.AddMinutes(10));
            }
        }

        private static void ScheduleTask(DateTime startAt)
        {
            TaskDefinition newTask = TaskService.Instance.NewTask();
            newTask.Settings.Enabled = true;
            newTask.Settings.ExecutionTimeLimit = TimeSpan.FromMinutes(10);
            newTask.Settings.RunOnlyIfNetworkAvailable = true;

            var timeTrigger = new TimeTrigger
            {
                Enabled = true,
                StartBoundary = startAt
            };

            newTask.Triggers.Add(timeTrigger);

            newTask.Actions.Add(new ExecAction
            {
                Path = "WeddingForward.ScheduleTaskExecutor.exe",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
            });

            Microsoft.Win32.TaskScheduler.Task registerTaskDefinition =
                TasksFolder.RegisterTaskDefinition($"TriggerExecutor", newTask);
        }

        public static TaskFolder TasksFolder
        {
            get
            {
                TaskFolder taskFolder = TaskService.Instance.RootFolder.SubFolders.FirstOrDefault(folder =>
                    folder.Name.Equals("WeddingForwardTasks"));

                if (taskFolder == null)
                {
                    taskFolder = TaskService.Instance.RootFolder.CreateFolder("WeddingForwardTasks");
                }

                return taskFolder;
            }
        }
    }
}
