using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class UserAccount : BaseEntity
    {
        public string UserId { get; set; }
        public virtual HexadoUser HexadoUser { get; set; }

        public virtual ICollection<Pub> OwnedPubs { get; set; } = new HashSet<Pub>();
    }
}