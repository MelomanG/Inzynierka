using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Functional.Maybe;
using Hexado.Db.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Core.Auth
{
    public class TokenFactory : ITokenFactory
    {
        private readonly JwtOptions _options;
        private readonly ILogger<TokenFactory> _logger;

        public TokenFactory(
            IOptions<JwtOptions> options,
            ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<TokenFactory>();
        }

        public Maybe<AccessToken> GenerateAccessToken(string email)
        {
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Email, email)
                        }),
                    Expires = DateTime.UtcNow.Add(_options.AccessTokenValidFor),
                    SigningCredentials = new SigningCredentials(HexadoTokenKey.Get(_options.Secret), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);

                var token = tokenHandler.WriteToken(securityToken);

                return new AccessToken(
                        token,
                        (int)_options.AccessTokenValidFor.TotalSeconds)
                    .ToMaybe();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to generate token for" +
                                     $"User: {email}");
                return Maybe<AccessToken>.Nothing;
            }
        }

        public Maybe<RefreshToken> GenerateRefreshToken(string userId, int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var token = Convert.ToBase64String(randomNumber);
            return new RefreshToken(
                    userId,
                    token,
                    DateTime.UtcNow.Add(_options.RefreshTokenValidFor))
                .ToMaybe();
        }
    }
}