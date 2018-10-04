using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi;
using com.ciclosoftware.infusionsoft.restapi.Authorization;
using com.ciclosoftware.infusionsoft.restapi.Contacts;
using com.ciclosoftware.infusionsoft.restapi.Tags;
using com.ciclosoftware.infusionsoft.restapi.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infusionsoft.TagExample.DomainModels
{
    public interface ITagExampleDomainModel
    {
        /// <summary>
        /// Get the currently configured clientId
        /// </summary>
        /// <returns></returns>
        string GetClientId();

        /// <summary>
        /// Retreives or creates the contact with the given email from Infusionsoft
        /// Applies the given tag to the contact, if it exists.
        /// </summary>
        /// <param name="email">The email of the Infusionsoft contact</param>
        /// <param name="tagId">The id of the Infusionsoft tag</param>
        /// <returns>Error description or null</returns>
        /// <returns></returns>
        Task<string> AddOrUpdateInfusionsoftContact(string email, int tagId);

        /// <summary>
        /// To authorize this webApp to access Infusionsoft in the users name. 
        /// </summary>
        /// <param name="authCode">To be retrieved through OAuth flow</param>
        /// <param name="redirectUrl">Optional, if OAuth happened through a popup window (not on the infusionsoft page).</param>
        /// <returns></returns>
        Task<string> Authorize(string authCode, string redirectUrl = null);

        /// <summary>
        /// To reseth authorization (delete token)
        /// </summary>
        void Reset();

        /// <summary>
        /// To test the current authorization.
        /// </summary>
        /// <returns>Result message (can be good or bad).</returns>
        Task<string> Test();

        Task RefreshToken();
        Task<List<InfusionsoftTag>> GetAllTags();
    }

    public class TagExampleDomainModel : ITagExampleDomainModel
    {
        private readonly ILogger<TagExampleDomainModel> _logger;
        private readonly IInfusionsoftAuthorization _infusionsoftAuthorization;
        private readonly IInfusionsoftContacts _infusionsoftContacts;
        private readonly IInfusionsoftUsers _infusionsoftUsers;
        private readonly IInfusionsoftTags _infusionsoftTags;
        private readonly ApiFactory _apiFactory;

        public TagExampleDomainModel(ILogger<TagExampleDomainModel> logger, 
            IInfusionsoftAuthorization infusionsoftAuthorization,
            IInfusionsoftContacts infusionsoftContacts,
            IInfusionsoftUsers infusionsoftUsers,
            IInfusionsoftTags infusionsoftTags,
            IConfiguration configuration)
        {
            _logger = logger;
            _infusionsoftAuthorization = infusionsoftAuthorization;
            _infusionsoftContacts = infusionsoftContacts;
            _infusionsoftUsers = infusionsoftUsers;
            _infusionsoftTags = infusionsoftTags;


            var infClientId = configuration["InfusionsoftClientId"];
            var infClientSecret = configuration["InfusionsoftClientSecret"];

            _apiFactory = new ApiFactory(infClientId, infClientSecret);
        }

        public string GetClientId()
        {
            return _apiFactory.ClientId;
        }

        /// <summary>
        /// Obviously this is just an example WebApp! 
        /// In a real world scenarion, token handling must be different! 
        /// </summary>
        private InfusionsoftToken InfusionsoftToken { get; set; }

        /// <summary>
        /// Always check if the token is still valid. If it's stale (will expire within the next hour),
        /// we refresh it through the Infusionsoft API. 
        /// </summary>
        private async Task<InfusionsoftToken> GetFreshToken()
        {
            if (InfusionsoftToken == null)
                return null;
            if (InfusionsoftToken.IssuedAt.AddSeconds(InfusionsoftToken.ExpiresIn) >= DateTime.UtcNow.AddHours(-1))
            {
                var newToken = await _infusionsoftAuthorization.RefreshToken(InfusionsoftToken.RefreshToken);
                InfusionsoftToken = newToken;
            }
            return InfusionsoftToken;
        }

        public async Task RefreshToken()
        {
            if (InfusionsoftToken == null)
                throw new InvalidDataException("No token available");
            var newToken = await _infusionsoftAuthorization.RefreshToken(InfusionsoftToken.RefreshToken);
            InfusionsoftToken = newToken;
        }

        public async Task<List<InfusionsoftTag>> GetAllTags()
        {
            var token = await GetFreshToken();
            var tags = await _infusionsoftTags.GetTags(token.AccessToken);
            return tags;
        }

        public async Task<string> AddOrUpdateInfusionsoftContact(string email, int tagId)
        {
            try
            {
                var contact = await GetContact(email);
                if (contact == null)
                {
                    return $"Contact '{email}' does not exists and could not be created";
                }
                var token = await GetFreshToken();

                var appliedTags = await _infusionsoftContacts.GetAppliedTags(token.AccessToken, contact.Id.Value);
                if (appliedTags.Any(appliedTag => appliedTag.Tag.Id.Equals(tagId)))
                {
                    _logger.LogInformation($"Tag '{tagId}' is already applied to user '{email}'");
                }
                else
                {
                    var success = await _infusionsoftContacts.ApplyTag(token.AccessToken, contact.Id.Value, tagId);
                    if (!success)
                    {
                        return $"Tag '{tagId}' could not be applied to '{email}'";
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception udpating infusionsoft contacts: {e.Message}");
                return "Unexpected error processing the user input";
            }
        }

        private async Task<InfusionsoftContact> GetContact(string email)
        {
            var token = await GetFreshToken();
            var contacts = await _infusionsoftContacts.GetContacts(token.AccessToken, email);

            InfusionsoftContact contact;
            if (contacts.Contacts.Count == 0)
            {
                //we have to create it
                var newContact = new InfusionsoftContact
                {
                    EmailAddresses = new List<EmailAddress>(new[] { new EmailAddress { Email = email, Field = "EMAIL1" } })
                };
                contact = await _infusionsoftContacts.CreateContact(token.AccessToken, newContact);
            }
            else
            {
                contact = contacts.Contacts[0];
            }
            return contact;
        }

        public async Task<string> Authorize(string authCode, string redirectUrl = null)
        {
            try
            {
                redirectUrl = redirectUrl ?? "null";
                var token = await _infusionsoftAuthorization.GetToken(authCode, redirectUrl);
                InfusionsoftToken = token;
                return token.Account;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception getting token: {ex.Message}");
                throw new ApplicationException("Exception authorizing with code " + authCode, ex);
            }
        }

        public void Reset()
        {
            InfusionsoftToken = null;
        }

        public async Task<string> Test()
        {
            try
            {
                if (InfusionsoftToken == null)
                {
                    return "No token provided";
                }
                var token = await GetFreshToken();
                var userInfo = await _infusionsoftUsers.GetUserInfo(token.AccessToken);
                if (userInfo != null)
                {
                    _logger.LogInformation($"Infusionsoft UserInfo retrieved: {userInfo.Email}");
                    var name = userInfo.GivenName ?? userInfo.Email;
                    var msg =
                        $"Hello {name}, you successfully authorized access for the account '{InfusionsoftToken.Account}'";
                    return msg;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception accessing Infusionsoft: {e.Message}");
            }
            return "Infusionsoft test failed";
        }
    }
}