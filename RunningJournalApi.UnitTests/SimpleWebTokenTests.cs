using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using NUnit.Framework;

namespace RunningJournalApi.UnitTests
{
    public class SimpleWebTokenTests
    {
        [Test]
        public void SutIsIteratorOfClaims()
        {
            var sut = new SimpleWebToken();
            Assert.IsInstanceOf<IEnumerable<Claim>>(sut);
        }

        [Test]
        public void SutYieldsInjectedClaims()
        {
            var expected = new[]
            {
                new Claim("foo", "bar"),
                new Claim("baz", "qux"),
                new Claim("quux", "corge")
            };
            var sut = new SimpleWebToken(expected);
            Assert.True(expected.SequenceEqual(sut));
            Assert.True(expected.SequenceEqual(sut.OfType<object>()));
        }

        [Test]
        [TestCase(new string[0], "")]
        [TestCase(new[] {"foo|bar"}, "foo=bar")]
        [TestCase(new[] {"foo|bar", "baz|qux"}, "foo=bar&baz=qux")]
        public void ToStringReturnCorrectResult(string[] keysAndValues, string expected)
        {
            var claim = keysAndValues
                .Select(s => s.Split('|'))
                .Select(a => new Claim(a[0], a[1]))
                .ToArray();
            var sut = new SimpleWebToken(claim);
            var actual = sut.ToString();
            Assert.AreEqual(actual, expected);
        }
    }
}