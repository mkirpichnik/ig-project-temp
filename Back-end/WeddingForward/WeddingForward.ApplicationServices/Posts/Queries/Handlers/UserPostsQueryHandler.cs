using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingForward.ApplicationServices.Automation.AccountSession;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;
using WeddingForward.ApplicationServices.Automation.Execution;
using WeddingForward.ApplicationServices.Posts.Models;
using WeddingForward.ApplicationServices.PythonAPI;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Posts.Queries.Handlers
{
    internal class UserPostsQueryHandler: IDataRequestHandler<UserPostsQuery, IReadOnlyList<UserPost>>
    {
        private readonly IScriptRunner _scriptRunner;

        private readonly IAccountsSessionManager _sessionManager;

        public UserPostsQueryHandler(IScriptRunner scriptRunner, IAccountsSessionManager sessionManager)
        {
            _scriptRunner = scriptRunner;
            _sessionManager = sessionManager;
        }

        public async Task<IReadOnlyList<UserPost>> ExecuteAsync(UserPostsQuery request)
        {
            ServiceResult<AccountSession> availableSession =
                await _sessionManager.GetAvailableSessionAsync()
                    .ConfigureAwait(false);

            if (availableSession.ResultType == ServiceResultType.Error)
            {
                return null;
            }

            AccountSession session = availableSession.Result;

            ServiceResult<UserPost[]> userPosts = await _scriptRunner.RunAsync(Scripts.UserPosts,new[]
            {
                //request.AuthData.ToString(),
                request.Username,
                request.Count.ToString()
            }, session).ConfigureAwait(false);

            return userPosts?.Result;
        }
    }
}
