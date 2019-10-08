using Hexado.Core.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using NUnit.Framework;

namespace Hexado.Core.Tests.Auth
{
    //TODO: Can't test it yet
    public class AccessTokenValidatorTests
    {
        [Test]
        public void ShouldReturnClaimsWhenTokenIsValid()
        {
            //GIVEN

            //WHEN

            //THEN
        }

        private class AccessTokenValidatorBuilder
        {
            public ISecurityTokenValidator SecurityTokenValidator { get; set; } =
                Substitute.For<ISecurityTokenValidator>();

            public ILoggerFactory LoggerFactory { get; set; } =
                Substitute.For<ILoggerFactory>();

            public AccessTokenValidator Build()
            {
                return new AccessTokenValidator(
                    SecurityTokenValidator,
                    LoggerFactory);
            }
        }
    }
}