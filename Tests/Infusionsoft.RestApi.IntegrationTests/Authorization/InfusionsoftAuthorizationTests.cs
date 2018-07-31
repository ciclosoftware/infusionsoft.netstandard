using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi.Authorization;
using Xunit;
using Xunit.Abstractions;

namespace Infusionsoft.RestApi.Authorization
{
    public class InfusionsoftAuthorizationTests : TestBase
    {
        private IInfusionsoftAuthorization _fixture;

        public InfusionsoftAuthorizationTests(ITestOutputHelper output) : base(output)
        {
            _fixture = ApiFactory.GetAuthenticationApi();
        }

        /// <summary>
        /// Add your dev Id and Secret into testsettings.json
        /// InlineData: a valid auth code from https://accounts.infusionsoft.com/app/central/home --> API Access --> Partner
        /// (using the same id than in MyKeys.cs!!)
        /// Remember that a code only works once! 
        /// </summary>
        [Fact]
        public void GetToken()
        {
            Task.Run(async () =>
            {
                var token = await _fixture.GetToken(ValidAuthCode, "null");
                Assert.NotNull(token);
                Output.WriteLine($"Token: {token.AccessToken}");
                Output.WriteLine($"Refresh Token: {token.RefreshToken}");
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add your dev Id and Secret into testsettings.json
        /// InlineData: a valid refreshToken, you can get it from the previous test or directly from https://accounts.infusionsoft.com/app/central/home --> API Access --> Personal
        /// (using the same developer id and secret than in MyKeys.cs!!)
        /// Remember that afterwards, only the new token and refreshToken are valid!
        /// </summary>
        [Fact]
        public void RefreshToken()
        {
            Task.Run(async () =>
            {
                var token = await _fixture.RefreshToken(ValidRefreshToken);
                Assert.NotNull(token);
                Output.WriteLine($"New Token: {token.AccessToken}");
                Output.WriteLine($"New Refresh Token: {token.RefreshToken}");
            }).GetAwaiter().GetResult();
        }
    }
}
