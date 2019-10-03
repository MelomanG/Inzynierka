using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Hexado.Db.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Core.Auth
{
    public interface IJwtTokenFactory
    {
        JwtTokenResult GenerateToken(HexadoUser user);
    }

    public class JwtTokenFactory : IJwtTokenFactory
    {
        private readonly JwtOptions _options;
        private readonly ILogger<JwtTokenFactory> _logger;

        public JwtTokenFactory(
            IOptions<JwtOptions> options,
            ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<JwtTokenFactory>();
        }

        public JwtTokenResult GenerateToken(HexadoUser user)
        {
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Email, user.Email)
                        }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(HexadoTokenKey.Get(_options.Secret), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                
                return string.IsNullOrWhiteSpace(token) 
                    ? JwtTokenResult.Invalid()
                    : JwtTokenResult.Valid(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to generate token for user: " +
                                     $"{user.Email}");
                return JwtTokenResult.Invalid();
            }
        }
    }
}