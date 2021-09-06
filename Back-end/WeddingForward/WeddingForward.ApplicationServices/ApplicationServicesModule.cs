using Microsoft.Extensions.DependencyInjection;
using WeddingForward.ApplicationServices.Accounts;
using WeddingForward.ApplicationServices.Automation;
using WeddingForward.ApplicationServices.CheckHistory;
using WeddingForward.ApplicationServices.Extensions;
using WeddingForward.ApplicationServices.Posts;

namespace WeddingForward.ApplicationServices
{
    public static class ApplicationServicesModule
    {
        public static IServiceCollection UseApplicationServices(this IServiceCollection collection)
        {
            return collection
                .UseAutomationModule()
                .UseAccountsModule()
                .UsePostsModule()
                .RegisterMapper()
                .UseCheckHistoryModule()
                .AddTransient<IDataRequestDispatcher, DataRequestDispatcher>();
        }
    }
}
