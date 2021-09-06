using Microsoft.Extensions.DependencyInjection;
using WeddingForward.ApplicationServices.Automation.AccountSession;
using WeddingForward.ApplicationServices.Automation.Execution;

namespace WeddingForward.ApplicationServices.Automation
{
    internal static class AutomationModule
    {
        public static IServiceCollection UseAutomationModule(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IAccountsSessionManager, AccountsSessionManager>()
                .AddTransient<IScriptRunner, ScriptRunner>();
        }
    }
}
