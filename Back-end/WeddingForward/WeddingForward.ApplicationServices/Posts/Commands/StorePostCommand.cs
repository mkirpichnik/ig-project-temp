using WeddingForward.ApplicationServices.Posts.Models;

namespace WeddingForward.ApplicationServices.Posts.Commands
{
    public class StorePostCommand: IDataRequest<bool>
    {
        public StorePostCommand(Post post)
        {
            Post = post;
        }

        public Post Post { get; }
    }
}
