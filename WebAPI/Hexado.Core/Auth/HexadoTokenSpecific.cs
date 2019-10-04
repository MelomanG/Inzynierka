using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Core.Auth
{
    public static class HexadoTokenSpecific
    {
        public static string GetAlgorithm => SecurityAlgorithms.HmacSha256Signature;

        public static SymmetricSecurityKey GetKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        public static TokenValidationParameters GetValidationParameters(string secret, bool validateLifeTime = true)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetKey(secret),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = validateLifeTime
            };
        }
    }
}