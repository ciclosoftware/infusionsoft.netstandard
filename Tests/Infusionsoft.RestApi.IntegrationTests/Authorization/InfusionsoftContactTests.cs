using System.Collections.Generic;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi;
using com.ciclosoftware.infusionsoft.restapi.Contacts;
using Xunit;
using Xunit.Abstractions;

namespace Infusionsoft.RestApi.IntegrationTests.Authorization
{
    public class InfusionsoftContactTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IInfusionsoftContacts _contactsApi;

        public InfusionsoftContactTests(ITestOutputHelper output)
        {
            _output = output;
            var api = ApiFactory.GetApiFactorySingleton(MyKeys.InfusionsoftClientId,
                MyKeys.InfusionsoftClientSecret);
            _contactsApi = api.GetContactsApi();
        }

        /// <summary>
        /// For the token, see 'InfusionsoftAuthorizationTests'
        /// </summary>
        [Theory]
        [InlineData("")]
        public void CrudContactTest(string token)
        {
            Task.Run(async () =>
            {
                //create 
                var newContact = new InfusionsoftContact
                {
                    GivenName = "Ben",
                    FamilyName = "Smith",
                    EmailAddresses = new List<EmailAddress>(new[] {new EmailAddress {Email = "ben@smith.xyz", Field = "EMAIL1" } })
                };
                newContact = await _contactsApi.CreateContact(token, newContact);
                Assert.NotNull(newContact);
                Assert.Equal("Ben", newContact.GivenName);

                //read one
                var ben = await _contactsApi.GetContact(token, newContact.Id);
                Assert.NotNull(ben);
                Assert.Equal("Ben", ben.GivenName);

                //read all
                var result = await _contactsApi.GetContacts(token);
                Assert.NotNull(result);
                Assert.NotNull(result.Contacts);
                _output.WriteLine($"Found {result.Contacts.Count} contacts");
                var newBen = result.Contacts.Find(c => c.Id == ben.Id);
                Assert.NotNull(newBen);
                Assert.Equal(ben.Id, newBen.Id);

                //update
                newBen.FamilyName = "Miller";
                var benMiller = await _contactsApi.UpdateContact(token, newBen);
                Assert.NotNull(benMiller);
                Assert.Equal(ben.Id, benMiller.Id);
                Assert.Equal("Miller", benMiller.FamilyName);

                //delete

            }).GetAwaiter().GetResult();
        }
    }
}