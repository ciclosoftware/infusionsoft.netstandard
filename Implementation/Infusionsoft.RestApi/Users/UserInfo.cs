using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Users
{
    public class UserInfo
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }
        [JsonProperty("given_name")]
        public string GivenName { get; set; }
        [JsonProperty("global_user_id")]
        public int GlobalUserId { get; set; }
        [JsonProperty("infusionsoft_id")]
        public string InfusionsoftId { get; set; }
        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }
        [JsonProperty("preferred_name")]
        public string PreferredName { get; set; }
        [JsonProperty("sub")]
        public string Sub { get; set; }

    }
}