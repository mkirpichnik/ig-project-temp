using Microsoft.Extensions.DependencyInjection;

namespace WeddingForward.ApplicationServices.CheckHistory
{
    internal static class CheckHistoryModule
    {
        public static IServiceCollection UseCheckHistoryModule(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IAccountsCheckHistory, AccountsCheckHistory>()
                .AddTransient<IPostCheckHistory, PostCheckHistory>();
        }
    }
}
