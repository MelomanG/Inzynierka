namespace Hexado.Db.Entities
{
    public class LikedPub: BaseEntity
    {
        public string PubId { get; set; }
        public virtual Pub Pub { get; set; }

        public string HexadoUserId { get; set; }
        public virtual HexadoUser HexadoUser { get; set; }
    }
}
