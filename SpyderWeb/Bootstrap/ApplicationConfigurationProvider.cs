using Microsoft.Extensions.Configuration;
using SpyderWeb.Helpers;

namespace SpyderWeb.Bootstrap
{
    public static class ApplicationConfigurationProvider
    {
        public static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(SolutionHelper.GetConfigRoot())
                .AddJsonFile("config.json")
                .AddJsonFile("filter.json", true)
                .Build();
        }
    }
}
