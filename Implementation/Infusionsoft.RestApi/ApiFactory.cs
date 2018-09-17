using System;
using com.ciclosoftware.infusionsoft.restapi.Authorization;
using com.ciclosoftware.infusionsoft.restapi.Contacts;
using com.ciclosoftware.infusionsoft.restapi.Service;
using com.ciclosoftware.infusionsoft.restapi.Users;

namespace com.ciclosoftware.infusionsoft.restapi
{
    public interface IApiFactory
    {
        string ClientId { get; set; }
        string ClientSecret { set; }
        IInfusionsoftAuthorization GetAuthenticationApi();
        IInfusionsoftContacts GetContactsApi();
        IInfusionsoftUsers GetUsersApi();
    }

    public class ApiFactory : IApiFactory
    {
        public const string ApiUrl = "https://api.infusionsoft.com/crm/rest/v1";
        public const string TokenUrl = "https://api.infusionsoft.com/token";

        private string _clientId;
        private string _clientSecret;
        private readonly InfusionsoftService _service;

        private ApiFactory()
        {
            _service = new InfusionsoftService();
        }

        public string ClientId
        {
            set { _clientId = value; }
            get
            {
                if (String.IsNullOrEmpty(_clientId))
                    throw new ApplicationException("You have to provide a client key in ApiFactory");
                return _clientId;
            }
        }

        public string ClientSecret
        {
            set { _clientSecret = value; }
            internal get
            {
                if (String.IsNullOrEmpty(_clientSecret))
                    throw new ApplicationException("You have to provide the client secret in ApiFactory");
                return _clientSecret;
            }
        }

        public IInfusionsoftAuthorization GetAuthenticationApi()
        {
            return new InfusionsoftAuthorization(_service);
        }

        public IInfusionsoftContacts GetContactsApi()
        {
            return new InfusionsoftContacts(_service);
        }

        public IInfusionsoftUsers GetUsersApi()
        {
            return new InfusionsoftUsers(_service);
        }

        public static ApiFactory Singleton { get; private set; }

        /// <summary>
        /// Setup the factory, and provide your clientId and secret (recommended on startup).
        /// </summary>
        /// <param name="clientId">developer.infusionsoft.com</param>
        /// <param name="clientSecret">developer.infusionsoft.com</param>
        /// <returns></returns>
        public static IApiFactory SetupApiFactorySingleton(string clientId, string clientSecret)
        {
            if (Singleton == null)
            {
                Singleton = new ApiFactory
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                };
            }
            return Singleton;
        }
    }
}