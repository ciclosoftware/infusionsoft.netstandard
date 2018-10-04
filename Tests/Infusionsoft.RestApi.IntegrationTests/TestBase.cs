using com.ciclosoftware.infusionsoft.restapi;
using Xunit.Abstractions;

namespace Infusionsoft.RestApi
{
    public abstract class TestBase
    {
        protected IApiFactory ApiFactory;
        protected readonly ITestOutputHelper Output;

        protected readonly string ValidToken;
        protected readonly string ValidAuthCode;
        protected readonly string ValidRefreshToken;

        protected TestBase(ITestOutputHelper output)
        {
            Output = output;

            var clientId = TestService.Configuration["InfusionsoftClientId"];
            var clientSecret = TestService.Configuration["InfusionsoftClientSecret"];
            ApiFactory = new ApiFactory(clientId, clientSecret);

            ValidToken = TestService.Configuration["ValidToken"];
            ValidAuthCode = TestService.Configuration["ValidAuthCode"];
            ValidRefreshToken = TestService.Configuration["ValidRefreshToken"];
        }
    }
}