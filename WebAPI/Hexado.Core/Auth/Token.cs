using Hexado.Db.Entities;

namespace Hexado.Core.Auth
{
    public class Token
    {
        public AccessToken AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public Token(
            AccessToken accessToken,
            RefreshToken refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken.Token;
        }
    }
}