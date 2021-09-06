using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Accounts.Commands.Handlers
{
    internal class StoreAccountInfoCommandHandler: DataBaseRequestHandler<StoreAccountInfoCommand, bool>
    {
        public StoreAccountInfoCommandHandler(IMapper mapper, WeddingForwardContext context)
            : base(mapper, context)
        {
        }

        public override async Task<bool> ExecuteAsync(StoreAccountInfoCommand request)
        {
            if (String.IsNullOrEmpty(request.Account?.Username))
            {
                throw new ArgumentNullException(nameof(request.Account.Username));
            }

            AccountSet data = Mapper.Map<AccountSet>(request.Account);

            AccountSet accountSet = await Context.Accounts
                .FirstOrDefaultAsync(set => set.Username.ToLower().Equals(request.Account.Username.ToLower()))
                .ConfigureAwait(false);

            if (accountSet == null)
            {
                await Context.Accounts.AddAsync(data).ConfigureAwait(false);
            }
            else
            {
                Context.Accounts.Update(accountSet);
            }

            int changes = await Context.SaveChangesAsync().ConfigureAwait(false);
            return changes > 0;
        }
    }
}
