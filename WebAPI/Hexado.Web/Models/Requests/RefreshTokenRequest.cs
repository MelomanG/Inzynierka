using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models.Requests
{
#nullable disable
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
#nullable restore
}