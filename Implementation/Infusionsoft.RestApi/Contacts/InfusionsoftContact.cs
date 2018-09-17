using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.ciclosoftware.infusionsoft.restapi.Contacts
{
    public class InfusionsoftContact
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("email_status")]
        public string EmailStatus { get; set; }

        [JsonProperty("phone_numbers")]
        public List<PhoneNumber> PhoneNumbers { get; set; }

        [JsonProperty("email_addresses")]
        public List<EmailAddress> EmailAddresses { get; set; }

        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty("date_created")]
        public string Created { get; set; }

        [JsonProperty("company")]
        public Company Company { get; set; }

        //just for single contacts, allcontacts don't include this
        [JsonProperty("tag_ids")]
        public List<int> TagIds { get; set; }

        [JsonProperty("anniversary")]
        public DateTime? Anniversary { get; set; }

        [JsonProperty("birthday")]
        public DateTime? Birthday { get; set; }

        [JsonProperty("contact_type")]
        public string ConctactType { get; set; }

        [JsonProperty("custom_fields")]
        public List<CustomField> CustomFields { get; set; }

        [JsonProperty("fax_numbers")]
        public List<FaxNumber> FaxNumbers { get; set; }

        [JsonProperty("job_title")]
        public string JobTitle { get; set; }

        [JsonProperty("lead_source_id")]
        public int? LeadSourceId { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("preferred_locale")]
        public string PreferredLocale { get; set; }

        [JsonProperty("preferred_name")]
        public string PreferredName { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("relationships")]
        public List<Relationship> Relationships { get; set; }

        [JsonProperty("social_accounts")]
        public List<SocialAccount> SocialAccounts { get; set; }

        [JsonProperty("source_type")]
        public string SourceType { get; set; }

        [JsonProperty("spouse_name")]
        public string SpouseName { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        /// <summary>
        /// For updating contacts (api-put)
        /// performs duplicate checking by one of the following options: Email, EmailAndName, 
        /// if a match is found using the option provided, the existing contact will be updated. 
        /// If an existing contact was not found using the duplicate_option provided, 
        /// a new contact record will be created.
        /// </summary>
        [JsonProperty("duplicate_option")]
        public string DuplicateOption { get; set; }

        /// <summary>
        /// You may opt-in or mark a Contact as Marketable by including the following field 
        /// in the request JSON with an opt-in reason.
        /// </summary>
        [JsonProperty("opt_in_reason")]
        public string OptInReason { get; set; }
    }
    public class SocialAccount
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Relationship
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("linked_contact_id")]
        public int LinkedContactId { get; set; }

        [JsonProperty("relationship_type_id")]
        public int RelationshipTypeId { get; set; }
    }

    public class FaxNumber
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class CustomField
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Company
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("company_name")]
        public string Name { get; set; }
    }

    public class PhoneNumber
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class EmailAddress
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class Address
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }

        [JsonProperty("line2")]
        public string Line2 { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}