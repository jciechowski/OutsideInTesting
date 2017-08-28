using System;
using System.Configuration;
using System.Dynamic;
using System.Net.Http;
using NUnit.Framework;
using RunningJournalApi;
using Simple.Data;

namespace OutsideInTesting
{
    public class HomeJsonTests
    {
        [Test]
        public void GetResponseReturnCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.True(response.IsSuccessStatusCode, "Actual status code: " + response.StatusCode);
            }
        }

        [Test]
        public void PostResponseReturnCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8500,
                    duration = TimeSpan.FromMinutes(44)
                };

                var response = client.PostAsJsonAsync("", json).Result;
                Assert.True(response.IsSuccessStatusCode, "Actual status code: " + response.StatusCode);
            }
        }

        [Test]
        public void GetAfterPostResponseReturnCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8000,
                    duration = TimeSpan.FromMinutes(41)
                };
                var expected = json.ToJObject();
                client.PostAsJsonAsync("", json).Wait();

                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsJsonAsync().Result;
                Assert.Contains(expected, actual.entries);
            }
        }

        [Test]
        [TestCase("foo")]
        [TestCase("bar")]
        [TestCase("baz")]
        public void GetRootReturnsCorrectEntryFromDatabase(string userName)
        {
            dynamic entry = new ExpandoObject();
            entry.time = DateTimeOffset.Now;
            entry.distance = 6000;
            entry.duration = TimeSpan.FromMinutes(31);

            var expected = ((object) entry).ToJObject();
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);
            db.User.Insert(Username: userName);
            var userId = db.User.FindAllByUserName(userName).Single().UserId;
            entry.UserId = userId;
            db.JournalEntry.Insert(entry);

            using (var client = HttpClientFactory.Create(userName))
            {
                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsJsonAsync().Result;
                Assert.Contains(expected, actual.entries);
            }
        }

        [SetUp]
        public void InstallDatabase()
        {
            Bootstrap.InstallDatabase();
        }

        [TearDown]
        public void UninstallDatabase()
        {
            Bootstrap.UninstallDatabase();
        }
    }
}