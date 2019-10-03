using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
    public class LoginUserModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}