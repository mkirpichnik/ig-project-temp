using System.Threading.Tasks;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices
{
    public interface IDataRequestDispatcher
    {
        Task<ServiceResult<TResult>> ExecuteAsync<TRequest, TResult>(TRequest request) where TRequest: IDataRequest<TResult>;
    }

    internal interface IDataRequestHandler<in TRequest, TResult> where TRequest: IDataRequest<TResult>
    {
        Task<TResult> ExecuteAsync(TRequest request);
    }

    public interface IDataRequest<TResult>
    {
    }

    interface IScriptRequest<TResult>: IDataRequest<TResult>
    {
        AccountSession Session { get; set; }
    }
}
