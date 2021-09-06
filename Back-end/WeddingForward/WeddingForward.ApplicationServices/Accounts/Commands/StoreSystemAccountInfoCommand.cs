
using WeddingForward.ApplicationServices.Accounts.Models;

namespace WeddingForward.ApplicationServices.Accounts.Commands
{
    internal class StoreSystemAccountInfoCommand: IDataRequest<bool>
    {
        public StoreSystemAccountInfoCommand(string login, string pass)
        {
            Login = login;
            Pass = pass;
        }

        public string Login { get; }

        public string Pass { get; }

        //public AccountAuthData AuthData { get; set; }
    }
}
