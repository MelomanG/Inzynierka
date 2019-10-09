using Hexado.Core.Auth;
using Hexado.Core.Models;

namespace Hexado.Web.Models.Responses
{
    public class TokenResponse
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}