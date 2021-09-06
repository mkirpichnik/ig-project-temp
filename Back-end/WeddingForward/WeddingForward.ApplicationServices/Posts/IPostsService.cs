using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Posts
{
    public interface IPostsService
    {
        Task<ServiceResult<IReadOnlyList<UserPost>>> GetUserPostsAsync(string username, int count = 5, bool useDb = true);

        Task<ServiceResult<Post>> GetPostInfo(string postId, bool checkDbInfo = true);
    }
}
