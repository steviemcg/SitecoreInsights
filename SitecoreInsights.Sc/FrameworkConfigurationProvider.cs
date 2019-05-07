using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace SitecoreInsights.Sc
{
    public class FrameworkConfigurationProvider : ConfigurationProvider, IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                Data.Add($"ConnectionStrings:{connectionString.Name}", connectionString.ConnectionString);
            }

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                Data.Add(key, ConfigurationManager.AppSettings[key]);
            }

            return this;
        }
    }
}