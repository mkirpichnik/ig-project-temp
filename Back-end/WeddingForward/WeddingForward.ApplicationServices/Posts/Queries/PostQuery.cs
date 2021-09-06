using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;
using WeddingForward.ApplicationServices.Posts.Models;

namespace WeddingForward.ApplicationServices.Posts.Queries
{
    internal class PostQuery: IScriptRequest<Post>
    {
        public PostQuery(string postId)
        {
            PostId = postId;
        }
        public string PostId { get; }

        public AccountSession Session { get; set; }
    }
}
