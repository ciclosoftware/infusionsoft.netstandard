using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Contacts
{
    public class InfusionsoftContactResults
    {
        [JsonProperty("contacts")]
        public List<InfusionsoftContact> Contacts { get; set; }
    }
}