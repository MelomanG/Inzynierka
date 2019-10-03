namespace Hexado.Core.Auth
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Secret { get; set; }
    }
}