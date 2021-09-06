using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WeddingForward.ApplicationServices.Accounts.Commands;
using WeddingForward.ApplicationServices.Accounts.Commands.Handlers;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Accounts.Queries;
using WeddingForward.ApplicationServices.Accounts.Queries.DataBase;
using WeddingForward.ApplicationServices.Accounts.Queries.DataBase.Handlers;
using WeddingForward.ApplicationServices.Accounts.Queries.Handlers;
using WeddingForward.ApplicationServices.Automation.AccountSession;

namespace WeddingForward.ApplicationServices.Accounts
{
    internal static class AccountsModule
    {
        public static IServiceCollection UseAccountsModule(this IServiceCollection collection)
        {
            return collection.AddTransient<IAccountsService, AccountService>()
                .AddTransient<Profile, AccountMapping>()
                .AddTransient<IDataRequestHandler<StoreAccountInfoCommand, bool>, StoreAccountInfoCommandHandler>()
                .AddTransient<IDataRequestHandler<FetchAccountQuery, Account>, FetchAccountQueryHandler>()
                //.AddTransient<IDataRequestHandler<StoreSystemAccountInfoCommand, bool>, StoreSystemAccountInfoCommandHandler>()
                //.AddTransient<IDataRequestHandler<SystemAccountInfoQuery, SystemAccount>, SystemAccountInfoQueryHandler>()
                .AddTransient<IDataRequestHandler<AccountSearchQuery, Account>, AccountSearchQueryHandler>();
            //.AddScoped<IAccountAuthService, AccountAuthService>();
        }
    }
}
