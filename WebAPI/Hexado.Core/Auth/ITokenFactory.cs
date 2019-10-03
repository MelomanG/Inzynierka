using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Core.Auth
{
    public interface ITokenFactory
    {
        Maybe<AccessToken> GenerateAccessToken(string email);
        Maybe<RefreshToken> GenerateRefreshToken(string userId, int size = 32);
    }
}