using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}