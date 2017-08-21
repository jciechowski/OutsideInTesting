using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

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
    }
}