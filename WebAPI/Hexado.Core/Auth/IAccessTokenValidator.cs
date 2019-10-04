using System.Collections.Generic;
using System.Security.Claims;
using Functional.Maybe;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Core.Auth
{
    public interface IAccessTokenValidator
    {
        Maybe<IEnumerable<Claim>> Validate(string accessToken, TokenValidationParameters parameters);
    }
}