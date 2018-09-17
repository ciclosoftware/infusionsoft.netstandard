using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Tags
{
    /// <summary>
    /// Result when calling all existing tags.
    /// </summary>
    public class InfusionsoftTagResults
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("tags")]
        public List<InfusionsoftTag> Tags { get; set; }
    }

    /// <summary>
    /// This is how the structure looks when asking for all existing tags.
    /// </summary>
    public class InfusionsoftTag
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public TagCategory Category { get; set; }
    }

    public class TagCategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class AppliedTagsResult
    {
        [JsonProperty("tags")]
        public List<TagApplication> Tags { get; set; }
    }

    /// <summary>
    /// The structure, when calling tags applied to a contact, is different
    /// than the structure of all existing tags. 
    /// </summary>
    public class TagApplication
    {
        [JsonProperty("date_applied")]
        public DateTime DateApplied { get; set; }
        [JsonProperty("tag")]
        public AppliedTag Tag { get; set; }
    }

    /// <summary>
    /// This is how the data looks for an applied tag for a contact.
    /// </summary>
    public class AppliedTag
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}