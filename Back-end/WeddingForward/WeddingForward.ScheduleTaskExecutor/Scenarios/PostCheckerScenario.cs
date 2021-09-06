using System;
using System.Threading.Tasks;

using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.ApplicationServices.Posts;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ScheduleTaskExecutor.Scenarios
{
    internal class PostCheckerScenario: ScenarioBase
    {
        private readonly IPostsService _postsService;

        public PostCheckerScenario(WeddingForwardContext db, IPostsService postsService)
            : base(db)
        {
            _postsService = postsService;
        }

        protected override async Task RunInternalAsync(string[] @params)
        {
            string accountId = @params[0];
            string postId = @params[1];

            ServiceResult<Post> serviceResult = await _postsService.GetPostInfo(postId, false).ConfigureAwait(false);
            if (serviceResult.ResultType == ServiceResultType.Error)
            {
                await Utils.ApplicationLogging.LogException(serviceResult, accountId, postId, Db).ConfigureAwait(false);

                return;
            }

            Post post = serviceResult.Result;
            if (post == null)
            {
                return;
            }

            var postCheckDetailsSet = new PostCheckDetailsSet
            {
                Id = Guid.NewGuid().ToString(),
                LikesCount = post.LikesCount,
                CommentsCount = post.CommentsCount
            };

            var checkerHistorySet = new PostCheckerHistorySet
            {
                AccountId = accountId,
                PostId = postId,
                CheckedAt = DateTime.UtcNow,
                PostCheckDetailsId = postCheckDetailsSet.Id
            };

            Db.PostCheckDetails.Add(postCheckDetailsSet);

            Db.PostCheckerHistory.Add(checkerHistorySet);

            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
