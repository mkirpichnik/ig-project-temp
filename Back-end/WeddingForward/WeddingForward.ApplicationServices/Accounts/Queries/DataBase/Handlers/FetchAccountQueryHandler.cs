using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Accounts.Queries.DataBase.Handlers
{
    internal class FetchAccountQueryHandler: IDataRequestHandler<FetchAccountQuery, Account>
    {
        private readonly WeddingForwardContext _dbContext;

        private readonly IMapper _mapper;

        public FetchAccountQueryHandler(WeddingForwardContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Account> ExecuteAsync(FetchAccountQuery request)
        {
            if (String.IsNullOrEmpty(request.Username))
            {
                return null;
            }

            AccountSet accountSet = await _dbContext.Accounts
                .FirstOrDefaultAsync(set => set.Username.ToLower().Equals(request.Username.ToLower()))
                .ConfigureAwait(false);

            Account account = _mapper.Map<Account>(accountSet);
            return account;
        }
    }
}
