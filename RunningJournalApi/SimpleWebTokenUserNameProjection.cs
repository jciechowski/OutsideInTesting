using System;
using System.Linq;
using System.Net.Http;

namespace RunningJournalApi
{
    public class SimpleWebTokenUserNameProjection : IUserNameProjection
    {
        public string GetUserName(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.Headers.Authorization == null)
                return null;
            if (request.Headers.Authorization.Scheme != "Bearer")
                return null;


            if (!SimpleWebToken.TryParse(request.Headers.Authorization.Parameter, out var swt))
                return null;

            return swt
                .Where(c => c.Type == "userName")
                .Select(c => c.Value).SingleOrDefault();
        }
    }
}