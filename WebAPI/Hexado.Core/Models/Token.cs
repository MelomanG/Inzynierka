using Hexado.Db.Entities;

namespace Hexado.Core.Models
{
    public class Token
    {
        public AccessToken AccessToken { get; }

        public RefreshToken RefreshToken { get; }

        public Token(
            AccessToken accessToken,
            RefreshToken refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}