using System.Collections.Generic;

namespace WeddingForward.ApplicationServices.Accounts.Models
{
    public class SystemAccount
    {
        public string Login { get; set; }

        public string Pass { get; set; }
    }

    public class SystemAccountsCollection
    {
        public IReadOnlyList<SystemAccount> Accounts { get; set; }
    }
}
