using System;
using com.ciclosoftware.infusionsoft.restapi.Authorization;
using com.ciclosoftware.infusionsoft.restapi.Service;

namespace com.ciclosoftware.infusionsoft.restapi
{
    public interface IApiFactory
    {
        string ApplicationKey { set; }
        string ApplicationSecret { set; }
        IInfusionsoftAuthorization GetInfusionsoftAuthorization();
    }

    public class ApiFactory : IApiFactory
    {
        public const string ApiUrl = "https://api.infusionsoft.com/crm/rest/v1";
        public const string TokenUrl = "https://api.infusionsoft.com/token";

        private string _applicationKey;
        private string _applicationSecret;
        private InfusionsoftService _service;

        private ApiFactory()
        {
            _service = new InfusionsoftService();
        }

        public string ApplicationKey
        {
            set { _applicationKey = value; }
            internal get
            {
                if (String.IsNullOrEmpty(_applicationKey))
                    throw new ApplicationException("You have to provide an application key in ApiFactory");
                return _applicationKey;
            }
        }

        public string ApplicationSecret
        {
            set { _applicationSecret = value; }
            internal get
            {
                if (String.IsNullOrEmpty(_applicationSecret))
                    throw new ApplicationException("You have to provide the application secret in ApiFactory");
                return _applicationSecret;
            }
        }

        public IInfusionsoftAuthorization GetInfusionsoftAuthorization()
        {
            return new InfusionsoftAuthorization(_service);
        }

        internal static ApiFactory Singleton { get; private set; }

        /// <summary>
        /// Create a new instance. Key and Secret can be defined now or later.
        /// </summary>
        /// <param name="applicationKey">developer.infusionsoft.com</param>
        /// <param name="applicationSecret">developer.infusionsoft.com</param>
        /// <returns></returns>
        public static IApiFactory GetApiFactorySingleton(string applicationKey = null, string applicationSecret = null)
        {
            if (Singleton == null)
            {
                Singleton = new ApiFactory
                {
                    ApplicationKey = applicationKey,
                    ApplicationSecret = applicationSecret
                };
            }
            return Singleton;
        }
    }
}