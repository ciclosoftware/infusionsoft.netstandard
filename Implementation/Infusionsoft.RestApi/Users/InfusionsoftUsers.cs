using System;
using System.Diagnostics;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi.Service;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Users
{
    public interface IInfusionsoftUsers
    {
        Task<UserInfo> GetUserInfo(string token);
    }

    public class InfusionsoftUsers : IInfusionsoftUsers
    {
        private readonly IInfusionsoftService _infusionsoftService;

        internal InfusionsoftUsers(IInfusionsoftService infusionsoftService)
        {
            _infusionsoftService = infusionsoftService;
        }

        public async Task<UserInfo> GetUserInfo(string token)
        {
            try
            {
                var contactResult = await _infusionsoftService.Get($"{ApiFactory.ApiUrl}/oauth/connect/userinfo", accessToken: token);
                if (string.IsNullOrEmpty(contactResult))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var info = JsonConvert.DeserializeObject<UserInfo>(contactResult);
                return info;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error calling and deserialising '/oauth/connect/userinfos': {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }
    }
}