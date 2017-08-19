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


    }
}