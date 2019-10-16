using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
#nullable disable
    public class PubModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
#nullable restore
}