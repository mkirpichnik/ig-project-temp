using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;

namespace WeddingForward.ApplicationServices.Accounts.Queries
{
    internal class AccountSearchQuery: IScriptRequest<Account>
    {
        public AccountSearchQuery(string accountName)
        {
            AccountName = accountName;
        }

        public string AccountName { get; }

        public AccountSession Session { get; set; }
    }
}
