using WeddingForward.ApplicationServices.Accounts.Models;

namespace WeddingForward.ApplicationServices.Accounts.Commands
{
    public class StoreAccountInfoCommand: IDataRequest<bool>
    {
        public StoreAccountInfoCommand(Account account)
        {
            Account = account;
        }

        public Account Account { get; }
    }
}
