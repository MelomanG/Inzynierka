using Hexado.Core.Models;

namespace Hexado.Web.Models.Responses
{
#nullable disable
    public class TokenResponse
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
#nullable restore
}