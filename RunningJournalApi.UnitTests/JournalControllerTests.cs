using System;
using System.Linq;
using System.Net;
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
        [TestCase("foo")]
        [TestCase("bar")]
        [TestCase("baz")]
        public void GetReturnsCorrectResult(string userName)
        {
            var userNameStub = new Mock<IUserNameProjection>();
            var queryStub = new Mock<IJournalEntriesQuery>();
            var commandDummy = new Mock<IAddJournalEntryCommand>();

            var sut = new JournalController(
                userNameStub.Object,
                queryStub.Object,
                commandDummy.Object)
            {
                Request = new HttpRequestMessage()
            };

            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            sut.Request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    new SimpleWebToken(new Claim("userName", userName)).ToString());

            userNameStub.Setup(m => m.GetUserName(sut.Request)).Returns(userName);

            var expected = new[]
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

            queryStub.Setup(q => q.GetJournalEntries(userName)).Returns(expected);

            var response = sut.Get();
            var actual = response.Content.ReadAsAsync<JournalModel>().Result;

            Assert.IsTrue(expected.SequenceEqual(actual.Entries));
        }

        [Test]
        public void GetWithoutUserNameReturnsCorrectResponse()
        {
            var userNameStub = new Mock<IUserNameProjection>();
            var queryStub = new Mock<IJournalEntriesQuery>();
            var commandDummy = new Mock<IAddJournalEntryCommand>();

            var sut = new JournalController(
                userNameStub.Object,
                queryStub.Object,
                commandDummy.Object)
            {
                Request = new HttpRequestMessage()
            };

            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            userNameStub.Setup(m => m.GetUserName(sut.Request)).Returns((string) null);

            var response = sut.Get();
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        [TestCase("foo")]
        [TestCase("bar")]
        [TestCase("baz")]
        public void PostInsertsEntry(string userName)
        {
            var userNameStub = new Mock<IUserNameProjection>();
            var queryStub = new Mock<IJournalEntriesQuery>();
            var cmdMock = new Mock<IAddJournalEntryCommand>();

            var sut = new JournalController(
                userNameStub.Object,
                queryStub.Object,
                cmdMock.Object)
            {
                Request = new HttpRequestMessage()
            };

            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());

            userNameStub.Setup(m => m.GetUserName(sut.Request)).Returns(userName);

            var entry = new JournalEntryModel();
            sut.Post(entry);
            cmdMock.Verify(c => c.AddJournalEntry(entry, userName));
        }

        [Test]
        public void PostWithoutUsernameReturnsCorrectResult()
        {
            var userNameStub = new Mock<IUserNameProjection>();
            var queryDummy = new Mock<IJournalEntriesQuery>();
            var cmdMock = new Mock<IAddJournalEntryCommand>();

            var sut = new JournalController(
                userNameStub.Object,
                queryDummy.Object,
                cmdMock.Object)
            {
                Request = new HttpRequestMessage()
            };

            sut.Request.Properties.Add(
                HttpPropertyKeys.HttpConfigurationKey,
                new HttpConfiguration());


            userNameStub.Setup(m => m.GetUserName(sut.Request)).Returns((string) null);

            var dummyEntry = new JournalEntryModel();
            var response = sut.Post(dummyEntry);
            cmdMock.Verify(m => m.AddJournalEntry(It.IsAny<JournalEntryModel>(), It.IsAny<string>()), Times.Never);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}