using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
#nullable disable
    public class PubModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public AddressModel Address { get; set; }
        public string ImagePath { get; set; }
    }
#nullable restore
}