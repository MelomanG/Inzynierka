using Hexado.Db.Constants;

namespace Hexado.Db.Entities
{
    public class BoardGameRate : BaseEntity
    {
        public UserRate UserRate { get; set; }
        public string Comment { get; set; }

        public string BoardGameId { get; set; }
        public virtual BoardGame BoardGame { get; set; }

        public string HexadoUserId { get; set; }
        public virtual HexadoUser HexadoUser { get; set; }
    }
}