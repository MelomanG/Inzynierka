using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
#nullable disable
    public class RateModel
    {
        [Required]
        public int UserRate { get; set; }

        public string Comment { get; set; }
    }
#nullable restore
}