using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using WeddingForward.ApplicationServices;
using WeddingForward.ApplicationServices.Accounts;
using WeddingForward.ApplicationServices.Accounts.Commands;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.ApplicationServices.Posts;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;
using WeddingForward.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace WeddingForward.Scheduler.Console
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
            // Show Checked Statistic
            WeddingForwardContext dbContext = ServiceProvider.GetService<WeddingForwardContext>();

            List<PostCheckerHistorySet> postCheckerHistorySets = dbContext.PostCheckerHistory.Include(set => set.PostCheckDetails).ToList();

            System.Console.WriteLine();
            System.Console.WriteLine();

            foreach (IGrouping<string, PostCheckerHistorySet> historySets in postCheckerHistorySets.GroupBy(set => set.AccountId))
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"Account: {historySets.Key}");

                foreach (IGrouping<string, PostCheckerHistorySet> checkerHistorySets in historySets.GroupBy(set => set.PostId))
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"Post: https://www.instagram.com/p/{checkerHistorySets.Key}/");

                    foreach (PostCheckerHistorySet postCheckerHistorySet in checkerHistorySets)
                    {
                        stringBuilder.AppendLine(
                            $"\t Likes: {postCheckerHistorySet.PostCheckDetails.LikesCount}\t Comments: {postCheckerHistorySet.PostCheckDetails.CommentsCount}\t Checked At: {postCheckerHistorySet.CheckedAt}");
                    }

                    stringBuilder.AppendLine();
                }

                System.Console.WriteLine(stringBuilder.ToString());
            }

            System.Console.ReadKey();

            return;

            // Add accounts to check 

            IAccountsService accountsService = ServiceProvider.GetService<IAccountsService>();
            IDataRequestDispatcher dataRequestDispatcher = ServiceProvider.GetService<IDataRequestDispatcher>();

            Task<ServiceResult<Account>>[] tasks = new[]
            {
                //mariyazakir
                //theweddingbliss
                //justweddingbells
                //lovinghautecouture
                //weddingdressesguide

                accountsService.SearchForAccount("weddingdressesguide"),
                accountsService.SearchForAccount("lovinghautecouture"),
                accountsService.SearchForAccount("justweddingbells"),
                accountsService.SearchForAccount("theweddingbliss"),
                accountsService.SearchForAccount("mariyazakir")
            };

            ServiceResult<Account>[] serviceResults = await Task.WhenAll(tasks).ConfigureAwait(false);

            foreach (ServiceResult<Account> serviceResult in serviceResults)
            {
                if (serviceResult.ResultType == ServiceResultType.Error)
                {
                    System.Console.WriteLine(JsonConvert.SerializeObject(serviceResult));
                    continue;
                }

                Account account = serviceResult.Result;
                if (account == null)
                {
                    System.Console.WriteLine("account was not found");
                    continue;
                }

                account.PostsCount -= 1;

                var storeAccountInfoCommand = new StoreAccountInfoCommand(account);

                ServiceResult<bool> result = await dataRequestDispatcher
                    .ExecuteAsync<StoreAccountInfoCommand, bool>(storeAccountInfoCommand)
                    .ConfigureAwait(false);

                if (result.ResultType == ServiceResultType.Error)
                {
                    System.Console.WriteLine(JsonConvert.SerializeObject(serviceResult));
                }
            }

            return;


            // Add schedule for posts monitorin

            WeddingForwardContext db = ServiceProvider.GetService<WeddingForwardContext>();

            db.ScriptsSchedule.Add(new ScriptsScheduleSet
            {
                Id = Guid.NewGuid(),
                IsStarted = false,
                IsFinished = false,
                ScriptType = "AccountsMonitoring",
                Args = null,
                PlanedStart = DateTime.UtcNow
            });

            await db.SaveChangesAsync().ConfigureAwait(false);
            TaskDefinition newTask = TaskService.Instance.NewTask();
            newTask.Settings.Enabled = true;
            //newTask.Settings.ExecutionTimeLimit = TimeSpan.FromMinutes(10);
            newTask.Settings.RunOnlyIfNetworkAvailable = true;

            var timeTrigger = new TimeTrigger
            {
                Enabled = true,
                StartBoundary = DateTime.Now.AddMinutes(2)
            };

            newTask.Triggers.Add(timeTrigger);

            newTask.Actions.Add(new ExecAction
            {
                Path = @"C:\Test\Test\WeddingForward.ScheduleTaskExecutor.exe",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
            });

            try
            {
                Microsoft.Win32.TaskScheduler.Task registerTaskDefinition =
                    TasksFolder.RegisterTaskDefinition(
                        $"TriggerExecutor", newTask);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(e));
            }
        }

        private static TaskFolder TasksFolder
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

        private static async Task SearchForAccounts(IAccountsService accountsService, params string[] accounts)
        {
            foreach (string account in accounts)
            {
                ServiceResult<Account> searchForAccount =
                    await accountsService.SearchForAccount(account)
                        .ConfigureAwait(false);

                if (searchForAccount.ResultType == ServiceResultType.Error)
                {
                    throw searchForAccount.Exception;
                }
            }
        }
    }
}
