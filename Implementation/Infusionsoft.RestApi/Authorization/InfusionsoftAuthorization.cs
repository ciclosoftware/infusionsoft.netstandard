using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.ciclosoftware.infusionsoft.restapi.Service;
using Newtonsoft.Json;
using Slybot.Core.Api.Infusionsoft.Models;

namespace com.ciclosoftware.infusionsoft.restapi.Authorization
{
    public interface IInfusionsoftAuthorization
    {
        Task<InfusionsoftToken> GetToken(string user, string authorizationCode, string redirectUrl = null);
        Task<InfusionsoftToken> RefreshToken(InfusionsoftToken token);
    }

    public class InfusionsoftAuthorization : IInfusionsoftAuthorization
    {
        private readonly IInfusionsoftService _infusionsoftService;

        public InfusionsoftAuthorization(IInfusionsoftService infusionsoftService)
        {
            _infusionsoftService = infusionsoftService;
        }

        public async Task<InfusionsoftToken> GetToken(string user, string authorizationCode, string redirectUrl)
        {
            if (string.IsNullOrEmpty(authorizationCode))
                return null;
            var infAppKey = ApiFactory.Singleton.ApplicationKey;
            var infAppSecret = ApiFactory.Singleton.ApplicationSecret;
            var dataString =
                $"code={authorizationCode}&client_id={infAppKey}&client_secret={infAppSecret}&redirect_uri={HttpUtility.UrlEncode(redirectUrl)}&grant_type=authorization_code";
            try
            {
                var resultJson = await _infusionsoftService.Post("https://api.infusionsoft.com/token", dataString);
                var tokenData = JsonConvert.DeserializeObject<InfusionsoftToken>(resultJson);
                var newToken = GetFullToken(tokenData);
                return newToken;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error getting access token: {e.Message}, {e.InnerException?.Message}");
                throw;
            }
        }

        public async Task<InfusionsoftToken> RefreshToken(InfusionsoftToken token)
        {
            try
            {
                var dataString =$"grant_type=refresh_token&refresh_token={token.RefreshToken}";
                var infAppKey = ApiFactory.Singleton.ApplicationKey;
                var infAppSecret = ApiFactory.Singleton.ApplicationSecret;
                var alt1 =$"{infAppKey}:{infAppSecret}";
                var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(alt1));
                var alt = $"Basic {base64}";
                var resultJson = await _infusionsoftService.Post(ApiFactory.TokenUrl, dataString, altAuthHeader: alt);
                var tokenData = JsonConvert.DeserializeObject<InfusionsoftToken>(resultJson);
                var newToken = GetFullToken(tokenData);
                return newToken;
            }
            catch (Exception e)
            {
                var msg = $"Error refreshing access token: {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }

        private InfusionsoftToken GetFullToken(InfusionsoftToken tokenData)
        {
            var split = tokenData.Scope.Split('|');
            var newToken = new InfusionsoftToken
            {
                Account = split.Length > 1 ? split[1] : null,
                AccessToken = tokenData.AccessToken,
                ExpiresIn = tokenData.ExpiresIn,
                RefreshToken = tokenData.RefreshToken,
                Scope = tokenData.Scope,
                TokenType = tokenData.TokenType,
                IssuedAt = DateTime.UtcNow
            };
            return newToken;
        }
    }
}