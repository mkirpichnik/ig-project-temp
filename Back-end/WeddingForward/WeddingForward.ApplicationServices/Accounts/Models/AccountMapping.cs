using AutoMapper;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Accounts.Models
{
    internal class AccountMapping: Profile
    {
        public AccountMapping()
        {
            CreateMap<Account, AccountSet>();
            CreateMap<AccountSet, Account>();
        }
    }
}
