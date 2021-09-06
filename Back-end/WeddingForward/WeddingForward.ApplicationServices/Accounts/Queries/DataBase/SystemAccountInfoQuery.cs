using WeddingForward.ApplicationServices.Accounts.Models;

namespace WeddingForward.ApplicationServices.Accounts.Queries.DataBase
{
    internal class SystemAccountInfoQuery: IDataRequest<SystemAccount>
    {
        public SystemAccountInfoQuery(string login)
        {
            Login = login;
        }

        public string Login { get; }
    }
}
