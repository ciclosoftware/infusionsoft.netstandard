using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Contacts
{
    public class ApplyTagsRequest
    {
        [JsonProperty("tagIds")]
        public List<int> TagIds{ get; set; }
    }
    //"{\"114\":\"SUCCESS\"}"
    //public class ApplyTagsResponse
    //{
    //    [JsonProperty("key")]
    //    public string Key { get; set; }
    //}
}