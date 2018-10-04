using com.ciclosoftware.infusionsoft.restapi.Authorization;
using com.ciclosoftware.infusionsoft.restapi.Contacts;
using com.ciclosoftware.infusionsoft.restapi.Service;
using com.ciclosoftware.infusionsoft.restapi.Users;

namespace com.ciclosoftware.infusionsoft.restapi
{
    public interface IApiFactory
    {
        string ClientId { get; }
        IInfusionsoftAuthorization GetAuthenticationApi();
        IInfusionsoftContacts GetContactsApi();
        IInfusionsoftUsers GetUsersApi();
    }

    public class ApiFactory : IApiFactory
    {
        public const string ApiUrl = "https://api.infusionsoft.com/crm/rest/v1";
        public const string TokenUrl = "https://api.infusionsoft.com/token";

        private readonly InfusionsoftService _service;

        public ApiFactory(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;

            _service = new InfusionsoftService();
        }

        public string ClientId { get; }

        internal string ClientSecret { get; }

        public IInfusionsoftAuthorization GetAuthenticationApi()
        {
            return new InfusionsoftAuthorization(this, _service);
        }

        public IInfusionsoftContacts GetContactsApi()
        {
            return new InfusionsoftContacts(_service);
        }

        public IInfusionsoftUsers GetUsersApi()
        {
            return new InfusionsoftUsers(_service);
        }
    }
}