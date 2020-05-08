﻿using System.ComponentModel.DataAnnotations;
using Hexado.Db.Entities;

namespace Hexado.Web.Models
{
#nullable disable
    public class PubModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressModel Address { get; set; }
    }

#nullable restore
}