using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingForward.Api.Controllers.Extensions;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.CheckHistory;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsHistoryController: ControllerBase
    {
        private readonly IAccountsCheckHistory _accountsCheckHistory;

        private readonly IPostCheckHistory _postCheckHistory;

        public PostsHistoryController(IAccountsCheckHistory accountsCheckHistory, IPostCheckHistory postCheckHistory)
        {
            _accountsCheckHistory = accountsCheckHistory;
            _postCheckHistory = postCheckHistory;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> Accounts()
        {
            ServiceResult<IReadOnlyList<Account>> serviceResult =
                await _accountsCheckHistory.GetCheckedAccountsAsync()
                    .ConfigureAwait(false);

            serviceResult.ThrowExceptionOnErrorResult();

            IReadOnlyList<Account> accounts = serviceResult.Result;

            return Ok(accounts);
        }

        [HttpGet("account/{accountName}/posts")]
        public async Task<IActionResult> AccountPosts(string accountName)
        {
            ServiceResult<IReadOnlyList<Post>> serviceResult =
                await _postCheckHistory.GetAccountsPostsAsync(accountName)
                    .ConfigureAwait(false);

            serviceResult.ThrowExceptionOnErrorResult();

            IReadOnlyList<Post> posts = serviceResult.Result;

            return Ok(posts);
        }
    }
}
