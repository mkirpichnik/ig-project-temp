using System.Threading.Tasks;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Accounts
{
    public interface IAccountsService
    {
        Task<ServiceResult<Account>> SearchForAccount(string accountName);
    }
}
