using Hexado.Db.Constants;

namespace Hexado.Db.Entities
{
    public class PubRate : BaseEntity
    {
        public UserRate UserRate { get; set; }
        public string Comment { get; set; }

        public string PubId { get; set; }
        public virtual Pub Pub { get; set; }

        public string HexadoUserId { get; set; }
        public virtual HexadoUser HexadoUser { get; set; }
    }
}