using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace SitecoreInsights.Sc
{
    [UsedImplicitly]
    public class SetupDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAnalyticsViewRepository, AnalyticsViewRepository>();
            serviceCollection.AddSingleton<IConfiguration>(c => new ConfigurationBuilder().Add(new FrameworkConfigurationProvider()).Build());
        }
    }
}