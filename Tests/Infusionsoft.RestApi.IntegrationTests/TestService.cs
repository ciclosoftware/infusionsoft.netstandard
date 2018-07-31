using Microsoft.Extensions.Configuration;

namespace Infusionsoft.RestApi
{
    public static class TestService
    {
        public static IConfiguration Configuration
        {
            get
            {
                var configBuilder = new ConfigurationBuilder().AddJsonFile("testsettings.json", false, false);
                var configuration = configBuilder.Build();
                return configuration;
            }
        }
    }
}