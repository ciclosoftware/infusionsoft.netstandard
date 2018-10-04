using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi.Service;
using com.ciclosoftware.infusionsoft.restapi.Tags;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Contacts
{
    public interface IInfusionsoftContacts
    {
        Task<InfusionsoftContactResults> GetContacts(string token, string email = null);
        Task<InfusionsoftContact> GetContact(string token, int id);

        /// <summary>
        /// https://developer.infusionsoft.com/docs/rest/#!/Contact/createOrUpdateContactUsingPUT
        /// </summary>
        Task<InfusionsoftContact> CreateContact(string token, InfusionsoftContact contact, string optInReason = null);
       
        /// <summary>
        /// https://developer.infusionsoft.com/docs/rest/#!/Contact/createOrUpdateContactUsingPUT
        /// </summary>
        /// <param name="duplicateOption">Default 'Email' - can also be 'EmailAndName'</param>
        Task<InfusionsoftContact> UpdateContact(string token, InfusionsoftContact contact, string optInReason = null, string duplicateOption = "Email");

        Task<bool> DeleteContact(string token, int id);

        Task<List<TagApplication>> GetAppliedTags(string token, int contactId);

        Task<bool> ApplyTag(string token, int contactId, int tagId);
    }

    public class InfusionsoftContacts : IInfusionsoftContacts
    {
        private readonly IInfusionsoftService _infusionsoftService;

        public InfusionsoftContacts(IInfusionsoftService infusionsoftService)
        {
            _infusionsoftService = infusionsoftService;
        }

        public async Task<InfusionsoftContactResults> GetContacts(string token, string email = null)
        {
            try
            {
                var url = $"{ApiFactory.ApiUrl}/contacts";
                if (!string.IsNullOrEmpty(email))
                {
                    url = $"{url}?email={email}";
                }
                var contactResult = await _infusionsoftService.Get(url, accessToken: token);
                if (string.IsNullOrEmpty(contactResult))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var contacts = JsonConvert.DeserializeObject<InfusionsoftContactResults>(contactResult);


                return contacts;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error getting and deserialising '/contacts': {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }

        public async Task<InfusionsoftContact> GetContact(string token, int id)
        {
            try
            {
                var resultJson = await _infusionsoftService.Get($"{ApiFactory.ApiUrl}/contacts/{id}", accessToken: token);
                if (string.IsNullOrEmpty(resultJson))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var contact = JsonConvert.DeserializeObject<InfusionsoftContact>(resultJson);
                return contact;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error getting '/contacts' and applied tags: {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }

        public async Task<InfusionsoftContact> CreateContact(string token, InfusionsoftContact contact, string optInReason = null)
        {
            try
            {
                contact.OptInReason = optInReason;
                var json = JsonConvert.SerializeObject(contact, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var resultJson =
                    await _infusionsoftService.Post($"{ApiFactory.ApiUrl}/contacts",
                        contentType: ContentType.Json, data: json, accessToken: token);
                if (string.IsNullOrEmpty(resultJson))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var result = JsonConvert.DeserializeObject<InfusionsoftContact>(resultJson);
                return result;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error posting and deserialising '/contacts': {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }

        public async Task<InfusionsoftContact> UpdateContact(string token, InfusionsoftContact contact, string optInReason = null, string duplicateOption = "Email")
        {
            try
            {
                contact.OptInReason = optInReason;
                contact.DuplicateOption = duplicateOption;
                var json = JsonConvert.SerializeObject(contact, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var resultJson =
                    await _infusionsoftService.Put($"{ApiFactory.ApiUrl}/contacts", json, token);
                if (string.IsNullOrEmpty(resultJson))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var result = JsonConvert.DeserializeObject<InfusionsoftContact>(resultJson);
                return result;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error putting and deserialising '/contacts': {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }

        public async Task<bool> DeleteContact(string token, int id)
        {
            try
            {
                var resultJson =
                    await _infusionsoftService.Delete($"{ApiFactory.ApiUrl}/contacts/{id}", null, token);
                Debug.WriteLine($"Response for deleting contact {id}: {resultJson}");
                return true;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error deleting '/contacts/{id}': {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }
        public async Task<List<TagApplication>> GetAppliedTags(string token, int contactId)
        {
            try
            {
                var resultJson = await _infusionsoftService.Get($"{ApiFactory.ApiUrl}/contacts/{contactId}/tags", accessToken: token);
                if (string.IsNullOrEmpty(resultJson))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var tags = JsonConvert.DeserializeObject<AppliedTagsResult>(resultJson);
                return tags.Tags;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error getting and deserialising '/contacts/{contactId}/tags': {e.Message}, {e.InnerException?.Message}";
                throw new ApplicationException(msg, e);
            }
        }

        public async Task<bool> ApplyTag(string token, int contactId, int tagId)
        {
            try
            {
                var atr = new ApplyTagsRequest { TagIds = new List<int>(new[] {tagId}) };
                var json = JsonConvert.SerializeObject(atr, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var resultJson =
                    await _infusionsoftService.Post($"{ApiFactory.ApiUrl}/contacts/{contactId}/tags",
                        contentType: ContentType.Json, data: json, accessToken: token);
                if (string.IsNullOrEmpty(resultJson))
                {
                    throw new NullReferenceException("Api result is null or empty");
                }
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultJson);
                if (dict.ContainsKey($"{tagId}") && dict[$"{tagId}"].Equals("SUCCESS"))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                var msg =
                    $"[Infusionsoft] Unexpected error posting and deserialising '/contacts': {e.Message}, {e.InnerException?.Message}";
                Debug.WriteLine(msg);
                throw new ApplicationException(msg, e);
            }
        }
    }
}