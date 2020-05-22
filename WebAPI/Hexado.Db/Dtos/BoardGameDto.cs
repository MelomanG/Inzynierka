using System.Collections.Generic;
using Hexado.Db.Entities;

namespace Hexado.Db.Dtos
{
    public class BoardGameDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int FromAge { get; set; }
        public string ImagePath { get; set; }

        public string CategoryId { get; set; }
        public BoardGameCategoryDto Category { get; set; }
        public IEnumerable<BoardGameRate> BoardGameRates { get; set; }

        public int AmountOfLikes { get; set; }
    }
}
