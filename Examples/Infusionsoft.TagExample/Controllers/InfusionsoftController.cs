using System;
using System.Threading.Tasks;
using Infusionsoft.TagExample.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Infusionsoft.TagExample.Controllers
{
    public class UserInput
    {
        public string Email { get; set; }
        public int TagId { get; set; }
    }

    public class AuthorizeInput
    {
        public string AuthCode { get; set; }
        public string RedirectUrl { get; set; }
    }

    [Route("api/[controller]")]
    public class InfusionsoftController : Controller
    {
        private readonly ILogger<InfusionsoftController> _logger;
        private readonly ITagExampleDomainModel _tagExampleDomainModel;

        public InfusionsoftController(
            ILogger<InfusionsoftController> logger, 
            ITagExampleDomainModel tagExampleDomainModel)
        {
            _logger = logger;
            _tagExampleDomainModel = tagExampleDomainModel;
        }

        [HttpGet("[action]")]
        public ActionResult GetClientId()
        {
            var clientId = _tagExampleDomainModel.GetClientId();
            if (!string.IsNullOrEmpty(clientId))
            {
                return Ok(clientId);
            }
            return BadRequest("No clientId configured");
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Authorize([FromBody] AuthorizeInput authorizeInput)
        {

            try
            {
                var account =
                    await _tagExampleDomainModel.Authorize(authorizeInput.AuthCode, authorizeInput.RedirectUrl);
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception getting token: {ex.Message}");
            }
            return BadRequest("Error getting access token");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RefreshToken()
        {
            try
            {
                await _tagExampleDomainModel.RefreshToken();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error refreshing token: {e.Message}");
                return BadRequest("Error refreshing token");
            }
        }

        [HttpPost("[action]")]
        public ActionResult Reset()
        {
            _tagExampleDomainModel.Reset();
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> Test()
        {
            var msg = await _tagExampleDomainModel.Test();
            return Ok(msg);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SendUser([FromBody] UserInput userinput)
        {
            var error = await _tagExampleDomainModel.AddOrUpdateInfusionsoftContact(userinput.Email, userinput.TagId);
            if (string.IsNullOrEmpty(error))
            {
                return Ok();
            }
            return BadRequest(error);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllTags()
        {
            var tags = await _tagExampleDomainModel.GetAllTags();
            if (tags != null)
            {
                return Ok(tags);
            }
            return BadRequest("Error getting tags");
        }
    }
}
