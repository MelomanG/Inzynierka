using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Core.Auth
{
    public interface ITokenFactory
    {
        Maybe<Token> GenerateToken(HexadoUser user);
    }
}