using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Functional.Maybe;
using Hexado.Core.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Core.Auth
{
    public class AccessTokenValidator : IAccessTokenValidator
    {
        private readonly ISecurityTokenValidator _securityTokenValidator;
        private readonly ILogger<AccessTokenValidator> _logger;

        public AccessTokenValidator(
            ISecurityTokenValidator securityTokenValidator,
            ILoggerFactory loggerFactory)
        {
            _securityTokenValidator = securityTokenValidator;
            _logger = loggerFactory.CreateLogger<AccessTokenValidator>();
        }

        public Maybe<IEnumerable<Claim>> Validate(string accessToken, TokenValidationParameters parameters)
        {
            try
            {
                var tokenWithoutBearer = accessToken.Replace(ConstantKey.BearerWithSpace, string.Empty);
                var claimsPrincipal = _securityTokenValidator.ValidateToken(tokenWithoutBearer, parameters, out var securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;
                var result = !jwtSecurityToken?.Header.Alg.Equals(HexadoTokenSpecific.GetAlgorithm, StringComparison.InvariantCultureIgnoreCase);

                return result.HasValue
                    ? claimsPrincipal.Claims.ToMaybe()
                    : Maybe<IEnumerable<Claim>>.Nothing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured during token validation!");
                return Maybe<IEnumerable<Claim>>.Nothing;
            }
        }
    }
}