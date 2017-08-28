using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http.SelfHost;
using RunningJournalApi;

namespace OutsideInTesting
{
    public static class HttpClientFactory
    {
        public static HttpClient Create(string userName = "foo")
        {
            var baseAddress = new Uri("http://localhost:1234");
            var config = new HttpSelfHostConfiguration(baseAddress);
            new Bootstrap().Configure(config);
            var server = new HttpSelfHostServer(config);
            var client = new HttpClient(server);
            try
            {
                client.BaseAddress = baseAddress;
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer",
                        new SimpleWebToken(new Claim("userName", userName)).ToString());
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }
    }
}