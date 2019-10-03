using System;

namespace Hexado.Core.Auth
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Secret { get; set; }
        public TimeSpan AccessTokenValidFor { get; set; }
        public TimeSpan RefreshTokenValidFor { get; set; }
    }
}