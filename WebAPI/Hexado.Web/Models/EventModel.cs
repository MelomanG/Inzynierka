using System;
using System.ComponentModel.DataAnnotations;

namespace Hexado.Web.Models
{
#nullable disable
    public class EventModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public AddressModel Address { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public string BoardGameId { get; set; }

        public string PubId { get; set; }

        public string ImagePath { get; set; }
    }
#nullable enable
}