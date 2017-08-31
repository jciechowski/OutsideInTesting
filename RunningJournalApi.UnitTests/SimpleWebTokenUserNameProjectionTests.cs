using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using NUnit.Framework;

namespace RunningJournalApi.UnitTests
{
    public class SimpleWebTokenUserNameProjectionTests
    {
        [Test]
        [TestCase("foo")]
        [TestCase("bar")]
        [TestCase("baz")]
        public void GetUserNameFromProperSimpleWebTokenReturnsCorrectResult(string expected)
        {
            var sut = new SimpleWebTokenUserNameProjection();
            var request = new HttpRequestMessage();
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    new SimpleWebToken(new Claim("userName", expected)).ToString());

            var actual = sut.GetUserName(request);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserNameFromNullRequestThrows()
        {
            var sut = new SimpleWebTokenUserNameProjection();
            Assert.Throws<ArgumentNullException>(() => sut.GetUserName(null));
        }

        [Test]
        public void GetUserNameFromRequestWithourAuthorizationHeaderReturnsCorrectResult()
        {
            var sut = new SimpleWebTokenUserNameProjection();
            var request = new HttpRequestMessage();
            Assert.Null(request.Headers.Authorization);

            var actual = sut.GetUserName(request);

            Assert.Null(actual);
        }

        [Test]
        [TestCase("Invalid")]
        [TestCase("AlmostBearer")]
        [TestCase("CloseToBearer")]
        [TestCase("It-IsAny-Bearer")]
        public void GetUserNameFromRequestWithIncorrectAuthorizationSchemeReturnsCorrectResult(string invalidScheme)
        {
            var sut = new SimpleWebTokenUserNameProjection();
            var request = new HttpRequestMessage();
            request.Headers.Authorization = new AuthenticationHeaderValue(
                invalidScheme,
                new SimpleWebToken(new Claim("userName", "dummy")).ToString());

            var actual = sut.GetUserName(request);

            Assert.Null(actual);
        }

        [Test]
        public void GetUserNameFromInvalidSimpleWebTokenReturnsCorrectResult()
        {
            var sut = new SimpleWebTokenUserNameProjection();
            var request = new HttpRequestMessage();
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                "Invalid web token");

            var actual = sut.GetUserName(request);

            Assert.Null(actual);
        }

        [Test]
        public void GetUserNameFromSimpleWebTokenWithNoUserClaimReturnsCorrectResult()
        {
            var sut = new SimpleWebTokenUserNameProjection();
            var request = new HttpRequestMessage();
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                new SimpleWebToken(new Claim("someClaim", "dummy")).ToString());

            var actual = sut.GetUserName(request);
            Assert.Null(actual);
        }
    }
}