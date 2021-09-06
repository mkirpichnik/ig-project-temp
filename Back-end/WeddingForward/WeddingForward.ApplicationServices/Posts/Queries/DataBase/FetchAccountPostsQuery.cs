using System.Collections.Generic;
using WeddingForward.ApplicationServices.Posts.Models;

namespace WeddingForward.ApplicationServices.Posts.Queries.DataBase
{
    internal class FetchAccountPostsQuery: IDataRequest<IReadOnlyList<UserPost>>
    {
        public FetchAccountPostsQuery(string ownerUsername)
        {
            OwnerUsername = ownerUsername;
        }

        public string OwnerUsername { get; }
    }
}
