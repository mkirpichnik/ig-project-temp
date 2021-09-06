using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Helpers
{
    public static class Utils
    {
        public static class ApplicationLogging
        {
            public static ILoggerFactory Factory { get; set; }

            public static async Task<int> LogException(string error,
                string accountId,
                string postId,
                WeddingForwardContext context)
            {
                context.Logs.Add(new LogsSet
                {
                    DateTime = DateTime.UtcNow,
                    AccountId = accountId,
                    PostId = postId,
                    Error = error
                });

                return await context.SaveChangesAsync().ConfigureAwait(false);
            }

            public static Task<int> LogException<TResult>(ServiceResult<TResult> serviceResult,
                string accountId,
                string postId,
                WeddingForwardContext context)
            {
                return LogException(JsonConvert.SerializeObject(serviceResult), accountId, postId, context);
            }

            public static Task<int> LogException(string error,
                WeddingForwardContext context)
            {
                return LogException(error, null, null, context);
            }

            public static Task<int> LogException<TResult>(ServiceResult<TResult> serviceResult,
                string accountId,
                WeddingForwardContext context)
            {
                return LogException(serviceResult, accountId, null, context);
            }

            public static Task<int> LogException<TResult>(ServiceResult<TResult> serviceResult,
                WeddingForwardContext context)
            {
                return LogException(serviceResult, null, null, context);
            }
        }
    }
}
