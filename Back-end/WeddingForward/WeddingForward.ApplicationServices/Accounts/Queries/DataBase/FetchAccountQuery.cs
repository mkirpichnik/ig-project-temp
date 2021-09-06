using WeddingForward.ApplicationServices.Accounts.Models;

namespace WeddingForward.ApplicationServices.Accounts.Queries.DataBase
{
    internal class FetchAccountQuery: IDataRequest<Account>
    {
        public FetchAccountQuery(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}
