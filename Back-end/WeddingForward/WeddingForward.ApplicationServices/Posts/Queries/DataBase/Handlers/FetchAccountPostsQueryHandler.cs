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

namespace WeddingForward.ApplicationServices.Posts.Queries.DataBase.Handlers
{
    internal class FetchAccountPostsQueryHandler: DataBaseRequestHandler<FetchAccountPostsQuery, IReadOnlyList<UserPost>>
    {
        public FetchAccountPostsQueryHandler(IMapper mapper, WeddingForwardContext context)
            : base(mapper, context)
        {
        }

        public override async Task<IReadOnlyList<UserPost>> ExecuteAsync(FetchAccountPostsQuery request)
        {
            if (String.IsNullOrEmpty(request.OwnerUsername))
            {
                throw new ArgumentNullException(nameof(request.OwnerUsername));
            }

            IReadOnlyList<AccountLinkedPosts> linkedPostses = await Context.AccountLinkedPosts
                .Where(posts => posts.OwnerUsername.ToLower().Equals(request.OwnerUsername.ToLower()))
                .OrderBy(posts => posts.Order).ToListAsync().ConfigureAwait(false);

            IReadOnlyList<UserPost> userPosts = linkedPostses.Select(posts => Mapper.Map<UserPost>(posts)).ToList();

            return userPosts;
        }
    }
}
