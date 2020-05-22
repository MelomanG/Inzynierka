namespace Hexado.Db.Entities
{
    public class LikedBoardGame: BaseEntity
    {
        public string BoardGameId { get; set; }
        public virtual BoardGame BoardGame { get; set; }

        public string HexadoUserId { get; set; }
        public virtual HexadoUser HexadoUser { get; set; }
    }
}
