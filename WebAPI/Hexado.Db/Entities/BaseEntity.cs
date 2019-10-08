using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexado.Db.Entities
{
    public interface IBaseEntity
    {
        string Id { get; set; }
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
    }

    public abstract class BaseEntity : IBaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}