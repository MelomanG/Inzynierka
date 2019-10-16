namespace Hexado.Db.Entities
{
    public class PubBoardGame : BaseEntity
    {
        public string PubId { get; set; }
        public Pub Pub { get; set; }

        public string BoardGameId { get; set; }
        public BoardGame BoardGame { get; set; }
    }
}