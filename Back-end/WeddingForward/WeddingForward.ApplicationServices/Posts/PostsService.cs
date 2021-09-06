using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingForward.ApplicationServices.Automation.AccountSession;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.Posts.Queries;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Posts
{
    internal class PostsService: IPostsService
    {
        private readonly IDataRequestDispatcher _dataRequestDispatcher;

        private readonly IAccountsSessionManager _sessionManager;

        public PostsService(IDataRequestDispatcher dataRequestDispatcher, IAccountsSessionManager sessionManager)
        {
            _dataRequestDispatcher = dataRequestDispatcher;
            _sessionManager = sessionManager;
        }

        public async Task<ServiceResult<IReadOnlyList<UserPost>>> GetUserPostsAsync(string username, int count = 5, bool useDb = true)
        {
            var postsQuery = new UserPostsQuery(username, count);

            ServiceResult<IReadOnlyList<UserPost>> serviceResult = await _dataRequestDispatcher
                .ExecuteAsync<UserPostsQuery, IReadOnlyList<UserPost>>(postsQuery)
                .ConfigureAwait(false);

            return serviceResult;
        }

        public async Task<ServiceResult<Post>> GetPostInfo(string postId, bool checkDbInfo = true)
        {
            var postQuery = new PostQuery(postId);

            ServiceResult<Post> serviceResult = await _dataRequestDispatcher.ExecuteAsync<PostQuery, Post>(postQuery)
                .ConfigureAwait(false);

            return serviceResult;
        }
    }
}
