using System;
using System.Threading.Tasks;
using System.Web;
using com.ciclosoftware.infusionsoft.restapi.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infusionsoft.OAuthExample.Controllers
{
    [Route("api/[controller]")]
    public class InfusionsoftOAuthController : Controller
    {
        private readonly ILogger<InfusionsoftOAuthController> _logger;
        private readonly IInfusionsoftAuthorization _infusionsoftAuthorization;

        private readonly string _infClientId;
        private readonly string _infClientSecret;

        public InfusionsoftOAuthController(ILogger<InfusionsoftOAuthController> logger, IConfiguration configuration, IInfusionsoftAuthorization infusionsoftAuthorization)
        {
            _logger = logger;
            _infusionsoftAuthorization = infusionsoftAuthorization;

            _infClientId = configuration["InfusionsoftClientId"];
            _infClientSecret = configuration["InfusionsoftClientSecret"];
        }

        [HttpGet("[action]")]
        public string GetOAuthUrl([FromQuery] string callbackUrl)
        {

            var redirectUri = HttpUtility.UrlEncode(callbackUrl);
            var url =
                $"https://signin.infusionsoft.com/app/oauth/authorize?scope=full&redirect_uri={redirectUri}&response_type=code&client_id={_infClientId}";
            return url;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> AuthCallback([FromQuery] string code, [FromQuery] string originalurl)
        {
            try
            {
                var token = await _infusionsoftAuthorization.GetToken(code, originalurl);
                Program.InfusionsoftToken = token;
                //The UI never gets the token. That would be exposing it to unsafe environments. 
                //The user trusted this WebApp by authorizing it! So be trustworthy and protect the token!
                //We just return the infusionsoft account info name as a confirmation:
                return Ok(token.Account);
            }
            catch (Exception ex)
            {
                //don't expose too much error information in a public environment!
                var msg = $"AuthCallback Exception: {ex.Message}";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
        }
    }
}
