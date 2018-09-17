using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi.Service;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Tags
{
    public interface IInfusionsoftTags
    {
        Task<List<InfusionsoftTag>> GetTags(string token);
    }

    public class InfusionsoftTags : IInfusionsoftTags
    {
        private readonly IInfusionsoftService _infusionsoftService;

        public InfusionsoftTags(IInfusionsoftService infusionsoftService)
        {
            _infusionsoftService = infusionsoftService;
        }

        public async Task<List<InfusionsoftTag>> GetTags(string token)
        {
            try
            {
                var resultJson = await _infusionsoftService.Get($"{ApiFactory.ApiUrl}/tags", accessToken: token);
                if (string.IsNullOrEmpty(resultJson))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var tagResult = JsonConvert.DeserializeObject<InfusionsoftTagResults>(resultJson);
                return tagResult.Tags;
            }
            catch (Exception e)
            {
                var msg = $"[Infusionsoft] Unexpected error getting and deserialising '/tags': {e.Message}, {e.InnerException?.Message}";
                throw new ApplicationException(msg, e);
            }
        }
    }
}