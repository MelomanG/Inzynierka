using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hexado.Db.Constants;

namespace Hexado.Web.Models
{
#nullable disable
    public class RateModel
    {
        [Required]
        public UserRate UserRate { get; set; }

        public string Comment { get; set; }
    }
#nullable restore
}