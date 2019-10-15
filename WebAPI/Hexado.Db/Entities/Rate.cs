using Hexado.Db.Constants;

namespace Hexado.Db.Entities
{
    public class Rate: BaseEntity
    {
        public string OwnerEmail { get; set; }
        public UserRate UserRate { get; set; }
        public string Comment { get; set; }

        public string BoardGameId { get; set; }
        public BoardGame BoardGame { get; set; }
    }
}