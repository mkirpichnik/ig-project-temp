using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingForward.ApplicationServices.PythonAPI;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Automation.Execution
{
    public interface IScriptRunner
    {
        Task<ServiceResult<TResult>> RunAsync<TResult>(PythonScript<TResult> script, IEnumerable<string> args, AccountSession.Models.AccountSession session) where TResult : class;

        Task<ServiceResult<TResult>> RunAsync<TResult>(PythonScript<TResult> script, AccountSession.Models.AccountSession session) where TResult : class;

        Task<ServiceResult<TResult>> RunAsync<TResult>(PythonScript<TResult> script, IEnumerable<string> args) where TResult : class;
    }
}
