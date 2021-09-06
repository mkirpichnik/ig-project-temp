using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingForward.Api.Controllers.Extensions;
using WeddingForward.ApplicationServices.Accounts;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Posts;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet("user/{accountName}")]
        public async Task<IActionResult> GetUserPosts(string accountName)
        {
            ServiceResult<IReadOnlyList<UserPost>> serviceResult =
                await _postsService.GetUserPostsAsync(accountName)
                    .ConfigureAwait(false);

            serviceResult.ThrowExceptionOnErrorResult();

            IReadOnlyList<UserPost> userPosts = serviceResult.Result;

            return Ok(userPosts);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(string postId)
        {
            ServiceResult<Post> serviceResult =
                await _postsService.GetPostInfo(postId)
                    .ConfigureAwait(false);

            serviceResult.ThrowExceptionOnErrorResult();

            Post userPosts = serviceResult.Result;

            return Ok(userPosts);
        }
    }
}
