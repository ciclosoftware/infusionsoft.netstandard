using System.Collections.Generic;
using System.Threading.Tasks;
using com.ciclosoftware.infusionsoft.restapi.Contacts;
using Xunit;
using Xunit.Abstractions;

namespace Infusionsoft.RestApi.Conctacts
{
    public class InfusionsoftContactTests : TestBase
    {
        private IInfusionsoftContacts _fixture;

        public InfusionsoftContactTests(ITestOutputHelper output) : base(output)
        {
            _fixture = ApiFactory.GetContactsApi();
        }

        /// <summary>
        /// How to get a token, see 'InfusionsoftAuthorizationTests'
        /// </summary>
        [Fact]
        public void GetContactsTest()
        {
            Task.Run(async () =>
            {
                var contactsResult = await _fixture.GetContacts(ValidToken);
                Assert.NotNull(contactsResult);
                var contact = await _fixture.GetContact(ValidToken, contactsResult.Contacts[0].Id);
                Assert.NotNull(contact);

            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// How to get a token, see 'InfusionsoftAuthorizationTests'
        /// </summary>
        //[Fact]
        public void CrudContactTest()
        {
            Task.Run(async () =>
            {
                //create 
                var newContact = new InfusionsoftContact
                {
                    GivenName = "Ben",
                    FamilyName = "Smith",
                    EmailAddresses = new List<EmailAddress>(new[] {new EmailAddress {Email = "ben@smith.de", Field = "EMAIL1" } })
                };
                newContact = await _fixture.CreateContact(ValidToken, newContact);
                Assert.NotNull(newContact);
                Assert.Equal("Ben", newContact.GivenName);

                //read one
                var ben = await _fixture.GetContact(ValidToken, newContact.Id);
                Assert.NotNull(ben);
                Assert.Equal("Ben", ben.GivenName);

                //read all
                var result = await _fixture.GetContacts(ValidToken);
                Assert.NotNull(result);
                Assert.NotNull(result.Contacts);
                Output.WriteLine($"Found {result.Contacts.Count} contacts");
                var newBen = result.Contacts.Find(c => c.Id == ben.Id);
                Assert.NotNull(newBen);
                Assert.Equal(ben.Id, newBen.Id);

                //update
                newBen.FamilyName = "Miller";
                var benMiller = await _fixture.UpdateContact(ValidToken, newBen);
                Assert.NotNull(benMiller);
                Assert.Equal(ben.Id, benMiller.Id);
                Assert.Equal("Miller", benMiller.FamilyName);

                //delete

            }).GetAwaiter().GetResult();
        }
    }
}