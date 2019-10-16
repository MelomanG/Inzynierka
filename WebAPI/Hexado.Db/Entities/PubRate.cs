using Hexado.Db.Constants;

namespace Hexado.Db.Entities
{
    public class PubRate : BaseEntity
    {
        public UserRate UserRate { get; set; }
        public string Comment { get; set; }

        public string PubId { get; set; }
        public Pub Pub { get; set; }

        public string HexadoUserId { get; set; }
        public HexadoUser HexadoUser { get; set; }
    }
}