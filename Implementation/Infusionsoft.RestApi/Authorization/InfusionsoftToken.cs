using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Slybot.Core.Api.Infusionsoft.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class InfusionsoftToken
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Seconds
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
