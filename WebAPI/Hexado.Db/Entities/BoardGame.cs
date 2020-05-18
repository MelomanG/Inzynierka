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
        public BoardGameCategory Category { get; set; }

        public ICollection<BoardGameRate> BoardGameRates { get; set; } = new HashSet<BoardGameRate>();
        public ICollection<PubBoardGame> PubBoardGames { get; set; } = new HashSet<PubBoardGame>();
    }
}