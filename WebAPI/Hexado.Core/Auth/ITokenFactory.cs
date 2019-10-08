using System.Collections.Generic;
using System.Security.Claims;
using Functional.Maybe;

namespace Hexado.Core.Auth
{
    public interface ITokenFactory
    {
        Maybe<Token> GenerateToken(string userId, IEnumerable<Claim> claims);
    }
}