using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ScheduleTaskExecutor.Scenarios
{
    internal interface IScenario
    {
        Task RunAsync(string[] @params);
    }

    internal abstract class ScenarioBase : IScenario
    {
        protected ScenarioBase(WeddingForwardContext db)
        {
            Db = db;
        }
        
        protected WeddingForwardContext Db
        {
            get;
        }

        public async Task RunAsync(string[] @params)
        {
            try
            {
                await RunInternalAsync(@params).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                string serializeObject = JsonConvert.SerializeObject(e);

                await Utils.ApplicationLogging.LogException(serializeObject, Db).ConfigureAwait(false);
            }
        }

        protected abstract Task RunInternalAsync(string[] @params);
    }
}
