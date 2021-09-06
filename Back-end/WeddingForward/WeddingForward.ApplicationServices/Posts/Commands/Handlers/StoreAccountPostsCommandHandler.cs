using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Posts.Commands.Handlers
{
    internal class StoreAccountPostsCommandHandler: DataBaseRequestHandler<StoreAccountPostsCommand, bool>
    {
        public StoreAccountPostsCommandHandler(IMapper mapper, WeddingForwardContext context)
            : base(mapper, context)
        {
        }

        public override async Task<bool> ExecuteAsync(StoreAccountPostsCommand request)
        {
            IReadOnlyList<UserPost> userPosts = request.UserPosts;
            if (userPosts?.Count <= 0)
            {
                return false;
            }

            IReadOnlyList<AccountLinkedPosts> linkedPostses = await Context.AccountLinkedPosts
                .Where(c => c.OwnerUsername.ToLower().Equals(request.OwnerUsername.ToLower())).ToListAsync()
                .ConfigureAwait(false);

            IReadOnlyList<UserPost> itemsToAdd = userPosts.Where(post => !linkedPostses.Any(posts =>
                    posts.OwnerUsername.ToLower().Equals(post.OwnerUsername.ToLower()) &&
                    posts.PostId.Equals(post.PostId)))
                .ToList();

            IEnumerable<AccountLinkedPosts> accountLinkedPostses =
                itemsToAdd.Select(post =>
                {
                    AccountLinkedPosts accountLinkedPosts = Mapper.Map<AccountLinkedPosts>(post);
                    accountLinkedPosts.Id = Guid.NewGuid();
                    return accountLinkedPosts;
                });

            await Context.AccountLinkedPosts.AddRangeAsync(accountLinkedPostses).ConfigureAwait(false);

            int changesAsync = await Context.SaveChangesAsync().ConfigureAwait(false);
            return changesAsync > 0;
        }
    }
}
