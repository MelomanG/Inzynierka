using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class BoardGame : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int FromAge { get; set; }
        public string ImagePath { get; set; }

        public string CategoryId { get; set; }
        public virtual BoardGameCategory Category { get; set; }

        public virtual ICollection<BoardGameRate> BoardGameRates { get; set; } = new HashSet<BoardGameRate>();
        public virtual ICollection<PubBoardGame> PubBoardGames { get; set; } = new HashSet<PubBoardGame>();
        public virtual ICollection<LikedBoardGame> LikedBoardGames { get; set; } = new HashSet<LikedBoardGame>();
    }
}