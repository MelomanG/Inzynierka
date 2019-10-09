using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Functional.Maybe;
using Hexado.Core.Models;
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

        public Maybe<Token> GenerateToken(string userId, IEnumerable<Claim> claims)
        {
            var accessToken = GenerateAccessToken(claims);
            if(!accessToken.HasValue)
                return Maybe<Token>.Nothing;

            var refreshToken = GenerateRefreshToken(userId);
            if (!refreshToken.HasValue)
                return Maybe<Token>.Nothing;

            return new Token(
                accessToken.Value,
                refreshToken.Value
            ).ToMaybe();
        }

        private Maybe<AccessToken> GenerateAccessToken(IEnumerable<Claim> claims)
        {
            try
            {
                var claimIdentity = new ClaimsIdentity(claims);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimIdentity,
                    Expires = DateTime.UtcNow.Add(_options.AccessTokenValidFor),
                    SigningCredentials = new SigningCredentials(HexadoTokenSpecific.GetKey(_options.Secret), HexadoTokenSpecific.GetAlgorithm)
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
                _logger.LogError(ex, "Unable to generate token!");
                return Maybe<AccessToken>.Nothing;
            }
        }

        private Maybe<RefreshToken> GenerateRefreshToken(string userId, int size = 32)
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