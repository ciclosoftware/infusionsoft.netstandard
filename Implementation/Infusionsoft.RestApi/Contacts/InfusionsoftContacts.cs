using System;
using System.Diagnostics;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi.Service;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Contacts
{
    public interface IInfusionsoftContacts
    {
        Task<InfusionsoftContactResults> GetContacts(string token);
        Task<InfusionsoftContact> GetContact(string token, int id);

        /// <summary>
        /// https://developer.infusionsoft.com/docs/rest/#!/Contact/createOrUpdateContactUsingPUT
        /// </summary>
        /// <param name="duplicateOption">Default 'Email' - can also be 'EmailAndName'</param>
        Task<InfusionsoftContact> CreateContact(string token, InfusionsoftContact contact, string optInReason = null, string duplicateOption = "Email");
       
        /// <summary>
        /// https://developer.infusionsoft.com/docs/rest/#!/Contact/createOrUpdateContactUsingPUT
        /// </summary>
        /// <param name="duplicateOption">Default 'Email' - can also be 'EmailAndName'</param>
        Task<InfusionsoftContact> UpdateContact(string token, InfusionsoftContact contact, string optInReason = null, string duplicateOption = "Email");

        Task<bool> DeleteContact(string token, int id);
    }

    internal class InfusionsoftContacts : IInfusionsoftContacts
    {
        private readonly IInfusionsoftService _infusionsoftService;

        internal InfusionsoftContacts(IInfusionsoftService infusionsoftService)
        {
            _infusionsoftService = infusionsoftService;
        }

        public async Task<InfusionsoftContactResults> GetContacts(string token)
        {
            try
            {
                var contactResult = await _infusionsoftService.Get($"{ApiFactory.ApiUrl}/contacts", accessToken: token);
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
                    $"[Infusionsoft] Unexpected error calling and deserialising '/contacts': {e.Message}, {e.InnerException?.Message}";
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

        public async Task<InfusionsoftContact> CreateContact(string token, InfusionsoftContact contact, string optInReason = null, string duplicateOption = "Email")
        {
            try
            {
                //contact.OptInReason = optInReason;
                //contact.DuplicateOption = duplicateOption;
                var json = JsonConvert.SerializeObject(contact);
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
                var json = JsonConvert.SerializeObject(contact);
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
    }
}