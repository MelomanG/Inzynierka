using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
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
    }
}