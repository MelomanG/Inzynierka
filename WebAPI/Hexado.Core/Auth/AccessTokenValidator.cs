using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Functional.Maybe;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Core.Auth
{
    public class AccessTokenValidator : IAccessTokenValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly ILogger<AccessTokenValidator> _logger;

        public AccessTokenValidator(
            JwtSecurityTokenHandler jwtSecurityTokenHandler,
            ILoggerFactory loggerFactory)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            _logger = loggerFactory.CreateLogger<AccessTokenValidator>();
        }

        public Maybe<IEnumerable<Claim>> Validate(string accessToken, TokenValidationParameters parameters)
        {
            try
            {
                var tokenWithoutBearer = accessToken.Replace("Bearer ", string.Empty);
                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(tokenWithoutBearer, parameters, out var securityToken);

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