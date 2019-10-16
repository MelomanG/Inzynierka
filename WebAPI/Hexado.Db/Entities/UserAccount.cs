using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class UserAccount : BaseEntity
    {
        public string UserId { get; set; }
        public HexadoUser HexadoUser { get; set; }

        public ICollection<Pub> OwnedPubs { get; set; } = new HashSet<Pub>();
    }
}