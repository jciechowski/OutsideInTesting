using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Hosting;
using OutsideInTesting;

namespace RunningJournalApi.UnitTests
{
    public class JournalControllerTests
    {
        [Test]
        public void GetReturnsCorrectResult()
        {
            var queryStub = new Mock<IJournalEntriesQuery>();
            var commandDummy = new Mock<IAddJournalEntryCommand>();

            var sut = new JournalController(queryStub.Object, commandDummy.Object)
            {
                Request = new HttpRequestMessage()
            };

            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            sut.Request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    new SimpleWebToken(new Claim("userName", "foo")).ToString());

            var expected = new []
            {
                new JournalEntryModel
                {
                    Distance = 1000,
                    Time = new DateTime(2017, 03, 04),
                    Duration = TimeSpan.FromMinutes(5)
                },
                new JournalEntryModel
                {
                    Distance = 2000,
                    Time = new DateTime(2017, 05, 13),
                    Duration = TimeSpan.FromMinutes(10)
                },
                new JournalEntryModel
                {
                    Distance = 3000,
                    Time = new DateTime(2017, 06, 25),
                    Duration = TimeSpan.FromMinutes(15)
                }
            };

            queryStub.Setup(q => q.GetJournalEntries("foo")).Returns(expected);

            var response = sut.Get();
            var actual = response.Content.ReadAsAsync<JournalModel>().Result;
            
            Assert.IsTrue(expected.SequenceEqual(actual.Entries));
        }
    }
}