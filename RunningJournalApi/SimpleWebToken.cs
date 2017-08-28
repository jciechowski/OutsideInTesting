using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RunningJournalApi
{
    public class SimpleWebToken : IEnumerable<Claim>
    {
        private readonly IEnumerable<Claim> _claims;

        public SimpleWebToken(params Claim[] claims)
        {
            _claims = claims;
        }

        public IEnumerator<Claim> GetEnumerator()
        {
            return _claims.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return _claims
                .Select(c => c.Type + "=" + c.Value)
                .DefaultIfEmpty(string.Empty)
                .Aggregate((x, y) => x + "&" + y);
        }

        public static bool TryParse(string tokenString, out SimpleWebToken token)
        {
            token = null;
            if (tokenString == string.Empty)
            {
                token = new SimpleWebToken();
                return true;
            }

            if (tokenString == null)
                return false;

            var claimPairs = tokenString.Split("&".ToArray());
            if (!claimPairs.All(x => x.Contains("=")))
                return false;

            var claims = claimPairs
                .Select(s => s.Split("=".ToArray()))
                .Select(a => new Claim(a[0], a[1]));
            token = new SimpleWebToken(claims.ToArray());

            return true;
        }
    }
}