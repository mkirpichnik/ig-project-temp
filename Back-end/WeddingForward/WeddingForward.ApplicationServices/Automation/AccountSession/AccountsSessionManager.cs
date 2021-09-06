using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Automation.Execution;
using WeddingForward.ApplicationServices.PythonAPI;
using WeddingForward.ApplicationServices.ServiceReults;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward.ApplicationServices.Automation.AccountSession
{
    internal class AccountsSessionManager : IAccountsSessionManager
    {
        private static readonly object AccountSessionSync = new object();

        private readonly WeddingForwardContext _dbContext;

        private readonly IScriptRunner _scriptRunner;

        private readonly SystemAccountsCollection _accountsCollection;

        public AccountsSessionManager(WeddingForwardContext dbContext, IScriptRunner scriptRunner,
            IOptions<SystemAccountsCollection> accountsCollection)
        {
            _dbContext = dbContext;
            _scriptRunner = scriptRunner;
            _accountsCollection = accountsCollection.Value;
        }

        public Task<ServiceResult<Models.AccountSession>> GetAccountSessionAsync(string accountId)
        {
            Models.AccountSession accountSession;

            lock (AccountSessionSync)
            {
                ServiceResult<Models.AccountSession> serviceResult = GetSessionFromDatabase(accountId).Result;
                if (serviceResult.ResultType == ServiceResultType.Error)
                {
                    return Task.FromResult(serviceResult);
                }

                accountSession = serviceResult.Result;
                if (accountSession == null)
                {
                    SystemAccount systemAccount = _accountsCollection.Accounts.FirstOrDefault(account => account.Login.Equals(accountId));
                    if (systemAccount == null)
                    {
                        throw new InvalidOperationException($"Credentials for account ({accountId}) was not found.");
                    }

                    ServiceResult<Models.AccountSession> authResult = AuthorizeAccount(systemAccount).Result;
                    if (authResult.ResultType == ServiceResultType.Error)
                    {
                        return Task.FromResult(authResult);
                    }

                    accountSession = authResult.Result;
                    if (accountSession != null)
                    {
                        StoreSessionToDatabase(accountSession).Wait();
                    }
                }

                if (accountSession != null)
                {
                    ServiceResult<bool> isSessionBlocked = IsSessionBlocked(accountSession).Result;
                    if (isSessionBlocked.ResultType == ServiceResultType.Error)
                    {
                        return Task.FromResult(isSessionBlocked.CreateErrorResult<bool, Models.AccountSession>());
                    }

                    if (!isSessionBlocked.Result)
                    {
                        MarkSessionAsBlocked(accountSession).Wait();

                        return Task.FromResult((ServiceResult<Models.AccountSession>)new ServiceErrorResult<Models.AccountSession>("Session is blocked"));
                    }
                }
            }

            return Task.FromResult(
                (ServiceResult<Models.AccountSession>) new SuccessServiceResult<Models.AccountSession>(accountSession));
        }

        public async Task<ServiceResult<bool>> CloseSession(string accountId)
        {
            try
            {
                AccountsSessionSet sessionSet = await _dbContext.AccountsSession
                    .FirstOrDefaultAsync(set => set.AccountId.Equals(accountId))
                    .ConfigureAwait(false);

                if (sessionSet == null)
                {
                    return new SuccessServiceResult<bool>(false);
                }

                _dbContext.Remove(sessionSet);
            
                int changes = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return new SuccessServiceResult<bool>(changes > 0);
            }
            catch (Exception e)
            {
                return new ServiceErrorResult<bool>(e.Message)
                {
                    Exception = e
                };
            }
        }

        public async Task<ServiceResult<Models.AccountSession>> GetAvailableSessionAsync()
        {
            ServiceResult<Models.AccountSession> result = null;

            foreach (SystemAccount systemAccount in _accountsCollection.Accounts)
            {
                result =
                    await GetAccountSessionAsync(systemAccount.Login)
                        .ConfigureAwait(false);

                if (result.ResultType == ServiceResultType.Error || result.Result == null)
                {
                    continue;
                }

                return result;
            }

            return result ?? new SuccessServiceResult<Models.AccountSession>(null);
        }

        private async Task<ServiceResult<Models.AccountSession>> GetSessionFromDatabase(string accountId)
        {
            if (String.IsNullOrEmpty(accountId))
            {
                throw new ArgumentNullException(nameof(accountId));
            }

            try
            {
                AccountsSessionSet sessionSet = await _dbContext.AccountsSession
                    .FirstOrDefaultAsync(set => set.AccountId.Equals(accountId) && set.IsAlive)
                    .ConfigureAwait(false);

                if (sessionSet == null)
                {
                    return new SuccessServiceResult<Models.AccountSession>(null);
                }

                return new SuccessServiceResult<Models.AccountSession>(new Models.AccountSession
                {
                    AccountId = sessionSet.AccountId,
                    CreatedDateTime = sessionSet.CreatedDateTime
                });
            }
            catch (Exception e)
            {
                return new ServiceErrorResult<Models.AccountSession>(e.Message)
                {
                    Exception = e
                };
            }
        }

        private async Task<ServiceResult<bool>> StoreSessionToDatabase(Models.AccountSession accountSession)
        {
            try
            {
                var accountsSessionSet = new AccountsSessionSet
                {
                    SessionId = accountSession.AccountId,
                    AccountId = accountSession.AccountId,
                    CreatedDateTime = DateTime.UtcNow,
                    IsAlive = true
                };

                await _dbContext.AccountsSession.AddAsync(accountsSessionSet).ConfigureAwait(false);

                int changes = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return new SuccessServiceResult<bool>(changes > 0);
            }
            catch (Exception e)
            {
                return new ServiceErrorResult<bool>(e.Message)
                {
                    Exception = e
                };
            }
        }

        private async Task<ServiceResult<bool>> MarkSessionAsBlocked(Models.AccountSession accountSession)
        {
            try
            {
                AccountsSessionSet sessionSet = await _dbContext.AccountsSession
                    .FirstOrDefaultAsync(set => set.AccountId.Equals(accountSession.AccountId))
                    .ConfigureAwait(false);

                if (sessionSet == null)
                {
                    return new SuccessServiceResult<bool>(false);
                }

                sessionSet.IsAlive = false;

                _dbContext.Update(sessionSet);

                int changes = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return new SuccessServiceResult<bool>(changes > 0);
            }
            catch(Exception e)
            {
                return new ServiceErrorResult<bool>(e.Message)
                {
                    Exception = e
                };
            }
        }

        private async Task<ServiceResult<Models.AccountSession>> AuthorizeAccount(SystemAccount account)
        {
            try
            {
                ServiceResult<Models.AccountSession> session = await _scriptRunner.RunAsync(Scripts.Auth, new[]
                {
                    account.Login,
                    account.Pass
                }).ConfigureAwait(false);

                return session;
            }
            catch (Exception e)
            {
                return new ServiceErrorResult<Models.AccountSession>(e.Message)
                {
                    Exception = e
                };
            }
        }

        private async Task<ServiceResult<bool>> IsSessionBlocked(Models.AccountSession session)
        {
            try
            {
                AccountsSessionSet sessionSet = await _dbContext.AccountsSession
                    .FirstOrDefaultAsync(set => set.SessionId.Equals(session.AccountId))
                    .ConfigureAwait(false);

                if (sessionSet == null || !sessionSet.IsAlive)
                {
                    return new SuccessServiceResult<bool>(false);
                }

                ServiceResult<CheckAccountResult> checkAccountResult =
                    await _scriptRunner.RunAsync(Scripts.CheckAuth, session)
                        .ConfigureAwait(false);

                if (checkAccountResult.ResultType == ServiceResultType.Error)
                {
                    return checkAccountResult.CreateErrorResult<CheckAccountResult, bool>();
                }

                return new SuccessServiceResult<bool>(checkAccountResult.Result.Success);
            }
            catch(Exception e)
            {
                return new ServiceErrorResult<bool>(e.Message)
                {
                    Exception = e
                };
            }
        }
    }
}
