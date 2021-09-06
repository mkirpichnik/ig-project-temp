using WeddingForward.ApplicationServices.Posts.Models;

namespace WeddingForward.ApplicationServices.Posts.Queries.DataBase
{
    internal class FetchPostQuery: IDataRequest<Post>
    {
        public FetchPostQuery(string postId)
        {
            PostId = postId;
        }

        public string PostId { get; }
    }
}
