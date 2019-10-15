using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
#nullable disable
    public class BoardGameModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 20)]
        public int? MinPlayers { get; set; }

        [Required]
        [Range(0, 20)]
        public int? MaxPlayers { get; set; }

        [Required]
        [Range(0,102)]
        public int? FromAge { get; set; }

        [Required]
        public string CategoryId { get; set; }
    }
#nullable restore
}