using System;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi;
using Xunit;

namespace Infusionsoft.RestApi.IntegrationTests.Authorization
{
    public class InfusionsoftAuthorizationTests
    {
        private IApiFactory _api;

        public InfusionsoftAuthorizationTests()
        {
            _api = ApiFactory.GetApiFactorySingleton(MyKeys.InfusionsoftClientId,
                MyKeys.InfusionsoftClientSecret);
        }

        [Theory]
        [InlineData("q9kz5pvff7hatv36spdszmen")]
        public void GetToken(string authCode)
        {
            Task.Run(async () =>
            {
                //var token = await _api.GetInfusionsoftAuthorization().GetToken(authCode, "https://accounts.infusionsoft.com/app/oauth/userToken");
                var token = await _api.GetInfusionsoftAuthorization().GetToken(authCode, "https://accounts.infusionsoft.com/app/central/home");
                Assert.NotNull(token);
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
