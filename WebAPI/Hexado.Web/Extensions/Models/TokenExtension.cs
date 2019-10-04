using Hexado.Core.Auth;
using Hexado.Web.Models.Responses;

namespace Hexado.Web.Extensions.Models
{
    public static class TokenExtension
    {
        public static TokenResponse ToTokenResponse(this Token token)
        {
            return new TokenResponse
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken.Token
            };
        }
    }
}
