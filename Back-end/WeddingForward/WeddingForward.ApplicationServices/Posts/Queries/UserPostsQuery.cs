using System.Collections.Generic;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;
using WeddingForward.ApplicationServices.Posts.Models;

namespace WeddingForward.ApplicationServices.Posts.Queries
{
    internal class UserPostsQuery : IScriptRequest<IReadOnlyList<UserPost>>
    {
        public UserPostsQuery(string username, int count)
        {
            Username = username;
            Count = count;
        }

        public string Username { get; }

        public int Count { get; }

        public AccountSession Session { get; set; }
    }
}
