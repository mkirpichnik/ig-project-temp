using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeddingForward.ApplicationServices.Accounts.Commands;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Accounts.Queries;
using WeddingForward.ApplicationServices.Accounts.Queries.DataBase;
using WeddingForward.ApplicationServices.Automation.AccountSession;
using WeddingForward.ApplicationServices.ServiceReults;

namespace WeddingForward.ApplicationServices.Accounts
{
    internal class AccountService: IAccountsService
    {
        private readonly IAccountsSessionManager _accountsSessionManager;

        private readonly IDataRequestDispatcher _requestDispatcher;

        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountsSessionManager accountsSessionManager, IDataRequestDispatcher requestDispatcher,
            ILogger<AccountService> logger)
        {
            _accountsSessionManager = accountsSessionManager;
            _requestDispatcher = requestDispatcher;
            _logger = logger;
        }

        public async Task<ServiceResult<Account>> SearchForAccount(string accountName)
        {
            var query = new AccountSearchQuery(accountName);

            try
            {
                _logger.LogDebug($"Trying to find information about {accountName} using script.");

                ServiceResult<Account> serviceResult = await _requestDispatcher
                    .ExecuteAsync<AccountSearchQuery, Account>(query)
                    .ConfigureAwait(false);

                _logger.LogDebug($"Script process result about {accountName}:\n {JsonConvert.SerializeObject(serviceResult)}.");

                return serviceResult;
            }
            catch (Exception e)
            {
                return new ServiceErrorResult<Account>(e.Message)
                {
                    Exception = e
                };
            }
        }
    }
}
