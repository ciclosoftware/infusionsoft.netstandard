using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi.Users;
using Xunit;
using Xunit.Abstractions;

namespace Infusionsoft.RestApi.Users
{
    public class InfusionsoftUserTests : TestBase
    {
        private IInfusionsoftUsers _fixture;

        public InfusionsoftUserTests(ITestOutputHelper output) : base(output)
        {
            _fixture = ApiFactory.GetUsersApi();
        }

        /// <summary>
        /// For the token, see 'InfusionsoftAuthorizationTests'
        /// </summary>
        [Fact]
        public void GetUserInfoTest()
        {
            Task.Run(async () =>
            {
                var res = await _fixture.GetUserInfo(ValidToken);
                Assert.NotNull(res);
                Assert.False(string.IsNullOrEmpty(res.InfusionsoftId));
                Assert.True(res.GlobalUserId > 0);

            }).GetAwaiter().GetResult();
        }
    }
}