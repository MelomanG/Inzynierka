using Hexado.Core.Auth;

namespace Hexado.Web.Models.Responses
{
    public class TokenResponse
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}