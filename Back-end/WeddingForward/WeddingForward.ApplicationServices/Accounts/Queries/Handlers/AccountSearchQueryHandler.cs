using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Automation.AccountSession;
using WeddingForward.ApplicationServices.Automation.AccountSession.Models;
using WeddingForward.ApplicationServices.Automation.Execution;
using WeddingForward.ApplicationServices.PythonAPI;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Accounts.Queries.Handlers
{
    internal class AccountSearchQueryHandler: IDataRequestHandler<AccountSearchQuery, Account>
    {
        private readonly IScriptRunner _scriptRunner;

        private readonly IAccountsSessionManager _sessionManager;

        public AccountSearchQueryHandler(IScriptRunner scriptRunner, IAccountsSessionManager sessionManager)
        {
            _scriptRunner = scriptRunner;
            _sessionManager = sessionManager;
        }

        public async Task<Account> ExecuteAsync(AccountSearchQuery request)
        {
            ServiceResult<AccountSession> availableSession =
                await _sessionManager.GetAvailableSessionAsync()
                    .ConfigureAwait(false);

            if (availableSession.ResultType == ServiceResultType.Error)
            {
                throw availableSession.Exception ?? new Exception(availableSession.ErrorMessage);
            }

            AccountSession session = availableSession.Result;

            ServiceResult<Account> serviceResult = await _scriptRunner.RunAsync(Scripts.AccountInfo, new[]
            {
                request.AccountName
            }, session).ConfigureAwait(false);
            
            return serviceResult?.Result;
        }
    }
}
