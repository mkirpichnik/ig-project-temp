using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.ApplicationServices.PythonAPI;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;

namespace WeddingForward.ApplicationServices.Automation.Execution
{
    internal class ScriptRunner: IScriptRunner
    {
        private static readonly object Sync = new Object();

        private static readonly IDictionary<string, Task> Tasks = new Dictionary<string, Task>();

        private readonly WeddingForwardContext _dbContext;

        public ScriptRunner(WeddingForwardContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<TResult>> RunAsync<TResult>(PythonScript<TResult> script, IEnumerable<string> args, AccountSession.Models.AccountSession session) where TResult : class
        {
            Task<ServiceResult<TResult>> task;

            if (session != null)
            {
                lock (Sync)
                {
                    Task specifiedTask;
                    if (Tasks.TryGetValue(session.AccountId, out specifiedTask))
                    {
                        specifiedTask?.Wait();
                    }

                    task = RunScriptAsync(script, args, session);

                    Tasks[session.AccountId] = task;
                }
            }
            else
            {
                task = RunScriptAsync(script, args, session);
            }

            ServiceResult<TResult> serviceResult = await task.ConfigureAwait(false);
            if (serviceResult.ResultType == ServiceResultType.Error)
            {
                await LogException(script, args, JsonConvert.SerializeObject(serviceResult)).ConfigureAwait(false);
            }

            return serviceResult;
        }

        public Task<ServiceResult<TResult>> RunAsync<TResult>(PythonScript<TResult> script, AccountSession.Models.AccountSession session) where TResult : class
        {
            return RunAsync(script, null, session);
        }

        public Task<ServiceResult<TResult>> RunAsync<TResult>(PythonScript<TResult> script, IEnumerable<string> args) where TResult : class
        {
            return RunAsync(script, args, null);
        }

        private static async Task<ServiceResult<TResult>> RunScriptAsync<TResult>(PythonScript<TResult> pythonScript, IEnumerable<string> args, AccountSession.Models.AccountSession session) where TResult : class
        {
            var logger = Utils.ApplicationLogging.Factory.CreateLogger(nameof(ScriptRunner));

            var cmdArgs = new List<string>();
            if (session != null)
            {
                cmdArgs.AddRange(new []
                {
                    session.AccountId
                });
            }

            if (args != null)
            {
                cmdArgs.AddRange(args);
            }

            string argsString = String.Join(' ', cmdArgs);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pythonScript.Path);

            logger.LogDebug($"Path = {path}, args = {argsString}");

            var processStartInfo = new ProcessStartInfo(path, argsString)
            {
                //UseShellExecute = false,
                RedirectStandardOutput = true,
                //CreateNoWindow = true,
                RedirectStandardError = true,
                //RedirectStandardInput = true,
                
            };

            logger.LogDebug("Start new process...");

            using (Process process = Process.Start(processStartInfo))
            {
                if (process == null)
                {
                    logger.LogDebug($"Process not created.");

                    return default;
                }

                logger.LogDebug($"Process has been created.");

                using (StreamReader resultReader = process.StandardOutput)
                {
                    string result = await resultReader.ReadToEndAsync().ConfigureAwait(false);
                    if (!String.IsNullOrEmpty(result))
                    {
                        result = result.Trim();
                        
                        logger.LogDebug($"Information from script has been received:\n {result}.");
                    }

                    using (StreamReader errorReader = process.StandardError)
                    {
                        string error = await errorReader.ReadToEndAsync().ConfigureAwait(false);
                        if (!String.IsNullOrEmpty(error))
                        {
                            var errorObject = new
                            {
                                Data = JsonConvert.SerializeObject(argsString),
                                Path = path,
                                Error = error
                            };

                            logger.LogDebug($"Error from script has been received:\n {JsonConvert.SerializeObject(errorObject)}.");

                            return new ServiceErrorResult<TResult>(JsonConvert.SerializeObject(errorObject));
                        }
                    }

                    if (String.IsNullOrEmpty(result))
                    {
                        return new ServiceErrorResult<TResult>("There is no response from the script.");
                    }

                    try
                    {
                        ScriptResult scriptResult = JsonConvert.DeserializeObject<ScriptResult>(result);
                        if (scriptResult.HasError)
                        {
                            return new ServiceErrorResult<TResult>(scriptResult.Error);
                        }

                        return new SuccessServiceResult<TResult>(
                            JsonConvert.DeserializeObject<TResult>(JsonConvert.SerializeObject(scriptResult.Result)));
                    }
                    catch (Exception e)
                    {
                        return new ServiceErrorResult<TResult>(e.Message)
                        {
                            Exception = e
                        };
                    }
                }
            }
        }

        private async Task LogException<TResult>(PythonScript<TResult> pythonScript, IEnumerable<string> args, string error) where TResult: class
        {
            var foo = new
            {
                ScriptInfo = JsonConvert.SerializeObject(pythonScript),
                Args = JsonConvert.SerializeObject(args ?? new String[0]),
                Error = error
            };

            await Utils.ApplicationLogging.LogException(JsonConvert.SerializeObject(foo), _dbContext)
                .ConfigureAwait(false);
        }

        private class ScriptResult
        {
            public object Result { get; set; }

            public bool HasError { get; set; }

            public string Error { get; set; }
        }
    }
}
