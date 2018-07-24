using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi;
using Xunit;
using Xunit.Abstractions;

namespace Infusionsoft.RestApi.IntegrationTests.Authorization
{
    public class InfusionsoftAuthorizationTests
    {
        private readonly ITestOutputHelper _output;
        private IApiFactory _api;

        public InfusionsoftAuthorizationTests(ITestOutputHelper output)
        {
            _output = output;
            _api = ApiFactory.GetApiFactorySingleton(MyKeys.InfusionsoftClientId,
                MyKeys.InfusionsoftClientSecret);
        }

        /// <summary>
        /// Add your dev Id and Secret into MyKeys.cs
        /// InlineData: a valid auth code from https://accounts.infusionsoft.com/app/central/home --> API Access --> Partner
        /// (using the same id than in MyKeys.cs!!)
        /// Remember that a code only works once! 
        /// </summary>
        [Theory]
        [InlineData("6ye3v3zury672wwvucpt7nas")]
        public void GetToken(string authCode)
        {
            Task.Run(async () =>
            {
                var token = await _api.GetInfusionsoftAuthorization().GetToken(authCode, "null");
                Assert.NotNull(token);
                _output.WriteLine($"Token: {token.AccessToken}");
                _output.WriteLine($"Refresh Token: {token.RefreshToken}");
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add your dev Id and Secret into MyKeys.cs
        /// InlineData: a valid refreshToken, you can get it from the previous test or directly from https://accounts.infusionsoft.com/app/central/home --> API Access --> Personal
        /// (using the same developer id and secret than in MyKeys.cs!!)
        /// Remember that afterwards, only the new token and refreshToken are valid!
        /// </summary>
        [Theory]
        [InlineData("6s7v7p295kdjgq2sxpacyvfb")]
        public void RefreshToken(string refreshToken)
        {
            Task.Run(async () =>
            {
                var token = await _api.GetInfusionsoftAuthorization().RefreshToken(refreshToken);
                Assert.NotNull(token);
                _output.WriteLine($"New Token: {token.AccessToken}");
                _output.WriteLine($"New Refresh Token: {token.RefreshToken}");
            }).GetAwaiter().GetResult();
        }
    }
}
