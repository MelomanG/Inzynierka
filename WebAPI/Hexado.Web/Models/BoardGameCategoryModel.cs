using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
#nullable disable
    public class BoardGameCategoryModel
    {
        [Required]
        public string Name { get; set; }
    }
#nullable restore
}