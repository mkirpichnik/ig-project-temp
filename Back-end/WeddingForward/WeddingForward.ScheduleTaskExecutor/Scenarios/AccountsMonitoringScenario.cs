using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeddingForward.ApplicationServices;
using WeddingForward.ApplicationServices.Accounts;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.ApplicationServices.Posts;
using WeddingForward.ApplicationServices.Posts.Commands;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ScheduleTaskExecutor.Scenarios
{
    internal class AccountsMonitoringScenario: ScenarioBase
    {
        private readonly IAccountsService _accountsService;

        private readonly IPostsService _postsService;

        private readonly IDataRequestDispatcher _dataRequestDispatcher;

        public AccountsMonitoringScenario(WeddingForwardContext db, IAccountsService accountsService,
            IPostsService postsService, IDataRequestDispatcher dataRequestDispatcher)
            : base(db)
        {
            _accountsService = accountsService;
            _postsService = postsService;
            _dataRequestDispatcher = dataRequestDispatcher;
        }

        protected override async Task RunInternalAsync(string[] @params)
        {
            var accountsPostInfo = await Db.Accounts.Select(set => new
            {
                UserName = set.Username,
                PostsCount = set.PostsCount
            }).ToListAsync().ConfigureAwait(false);

            Dictionary<string, int> dictionary = accountsPostInfo.ToDictionary(arg => arg.UserName, arg => arg.PostsCount);

            foreach (var account in dictionary.Keys)
            {
                ServiceResult<Account> serviceResult =
                    await _accountsService.SearchForAccount(account)
                        .ConfigureAwait(false);

                Account result = serviceResult.Result;

                if (result == null || serviceResult.ResultType == ServiceResultType.Error)
                {
                    await Utils.ApplicationLogging.LogException(serviceResult, account, null, Db).ConfigureAwait(false);

                    continue;
                }

                if (dictionary[account] == result.PostsCount)
                {
                    continue;
                }

                ServiceResult<IReadOnlyList<UserPost>> userPostsAsync =
                    await _postsService.GetUserPostsAsync(account, 1, false)
                        .ConfigureAwait(false);

                IReadOnlyList<UserPost> userPosts = userPostsAsync.Result;

                if (userPosts == null || userPosts.Count == 0 || userPostsAsync.ResultType == ServiceResultType.Error)
                {
                    if (userPostsAsync.ResultType == ServiceResultType.Error)
                    {
                        await Utils.ApplicationLogging.LogException(serviceResult, account, null, Db).ConfigureAwait(false);
                    }

                    continue;
                }

                UserPost userPost = userPosts.First();

                bool postExists = await Db.Posts.AnyAsync(p => p.PostId.Equals(userPost.PostId)).ConfigureAwait(false);
                if (!postExists)
                {
                    ServiceResult<Post> postInfo =
                        await _postsService.GetPostInfo(userPost.PostId, false)
                            .ConfigureAwait(false);

                    if (postInfo.ResultType == ServiceResultType.Error)
                    {
                        await Utils.ApplicationLogging
                            .LogException(serviceResult, userPost.OwnerUsername, userPost.PostId, Db)
                            .ConfigureAwait(false);

                        throw postInfo.Exception;
                    }

                    var storePostCommand = new StorePostCommand(postInfo.Result);

                    ServiceResult<bool> storePostResult = await _dataRequestDispatcher
                        .ExecuteAsync<StorePostCommand, bool>(storePostCommand).ConfigureAwait(false);

                    if (storePostResult.ResultType == ServiceResultType.Error)
                    {
                        await Utils.ApplicationLogging
                            .LogException(serviceResult, userPost.OwnerUsername, userPost.PostId, Db)
                            .ConfigureAwait(false);

                        throw storePostResult.Exception;
                    }

                    var storeAccountPostsCommand = new StoreAccountPostsCommand(userPost.OwnerUsername, new[]
                    {
                            new UserPost
                            {
                                PostId = userPost.PostId,
                                OrderNumber = -1,
                                OwnerUsername = userPost.OwnerUsername,
                                PostLink = userPost.PostLink
                            }
                        });

                    ServiceResult<bool> linkPostResult = await _dataRequestDispatcher
                        .ExecuteAsync<StoreAccountPostsCommand, bool>(storeAccountPostsCommand)
                        .ConfigureAwait(false);

                    if (linkPostResult.ResultType == ServiceResultType.Error)
                    {
                        await Utils.ApplicationLogging
                            .LogException(serviceResult, userPost.OwnerUsername, userPost.PostId, Db)
                            .ConfigureAwait(false);

                        throw linkPostResult.Exception;
                    }
                }

                string args = $"{account};{userPost.PostId}";

                bool anyAsync = await Db.ScriptsSchedule
                    .AnyAsync(set => set.ScriptType.Equals("PostChecker") && set.Args.Equals(args))
                    .ConfigureAwait(false);

                if (!anyAsync)
                {
                    Db.ScriptsSchedule.Add(new ScriptsScheduleSet
                    {
                        Id = Guid.NewGuid(),
                        IsStarted = false,
                        IsFinished = false,
                        ScriptType = "PostChecker",
                        Args = args,
                        PlanedStart = DateTime.UtcNow.AddMinutes(2)
                    });

                    Db.ScriptsSchedule.Add(new ScriptsScheduleSet
                    {
                        Id = Guid.NewGuid(),
                        IsStarted = false,
                        IsFinished = false,
                        ScriptType = "PostChecker",
                        Args = args,
                        PlanedStart = DateTime.UtcNow.AddHours(1)
                    });

                    Db.ScriptsSchedule.Add(new ScriptsScheduleSet
                    {
                        Id = Guid.NewGuid(),
                        IsStarted = false,
                        IsFinished = false,
                        ScriptType = "PostChecker",
                        Args = args,
                        PlanedStart = DateTime.UtcNow.AddDays(1)
                    });

                    await Db.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
