using System.Threading.Tasks;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Automation.AccountSession
{
    public interface IAccountsSessionManager
    {
        Task<ServiceResult<Models.AccountSession>> GetAccountSessionAsync(string accountId);

        Task<ServiceResult<bool>> CloseSession(string accountId);

        Task<ServiceResult<Models.AccountSession>> GetAvailableSessionAsync();
    }
}
