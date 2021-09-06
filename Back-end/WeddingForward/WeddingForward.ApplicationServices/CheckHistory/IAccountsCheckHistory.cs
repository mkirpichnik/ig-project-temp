using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.CheckHistory
{
    public interface IAccountsCheckHistory
    {
        Task<ServiceResult<IReadOnlyList<Account>>> GetCheckedAccountsAsync();
    }

    internal class AccountsCheckHistory : IAccountsCheckHistory
    {
        private readonly WeddingForwardContext _db;

        private readonly IMapper _mapper;

        public AccountsCheckHistory(WeddingForwardContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IReadOnlyList<Account>>> GetCheckedAccountsAsync()
        {
            IReadOnlyList<string> accountsIds = await _db.PostCheckerHistory.Select(set => set.AccountId)
                .Distinct()
                .ToListAsync()
                .ConfigureAwait(false);

            if (accountsIds.Count == 0)
            {
                return new SuccessServiceResult<IReadOnlyList<Account>>(new List<Account>(0));
            }

            IReadOnlyList<AccountSet> accountSets = await _db.Accounts.Where(set => accountsIds.Contains(set.Username))
                .ToListAsync()
                .ConfigureAwait(false);

            IReadOnlyList<Account> accounts = accountSets.Select(set => _mapper.Map<Account>(set)).ToList();

            return new SuccessServiceResult<IReadOnlyList<Account>>(accounts);
        }
    }
}
