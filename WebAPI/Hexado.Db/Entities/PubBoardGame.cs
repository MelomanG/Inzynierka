namespace Hexado.Db.Entities
{
    public class PubBoardGame : BaseEntity
    {
        public string PubId { get; set; }
        public virtual Pub Pub { get; set; }

        public string BoardGameId { get; set; }
        public virtual BoardGame BoardGame { get; set; }
    }
}