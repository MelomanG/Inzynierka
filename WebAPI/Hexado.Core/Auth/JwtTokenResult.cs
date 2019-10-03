namespace Hexado.Core.Auth
{
    public class JwtTokenResult
    {
        public string Token { get; private set; }
        public bool IsValid { get; private set; }

        public static JwtTokenResult Invalid()
        {
            return new JwtTokenResult
            {
                Token = string.Empty,
                IsValid = false
            };
        }

        public static JwtTokenResult Valid(string token)
        {
            return new JwtTokenResult
            {
                Token = token,
                IsValid = true
            };
        }
    }
}