using System.Collections.Generic;
using WeddingForward.ApplicationServices.Posts.Models;

namespace WeddingForward.ApplicationServices.Posts.Commands
{
    public class StoreAccountPostsCommand: IDataRequest<bool>
    {
        public StoreAccountPostsCommand(string ownerUsername, IReadOnlyList<UserPost> userPosts)
        {
            OwnerUsername = ownerUsername;
            UserPosts = userPosts;
        }

        public string OwnerUsername { get; }

        public IReadOnlyList<UserPost> UserPosts { get;  }
    }
}
