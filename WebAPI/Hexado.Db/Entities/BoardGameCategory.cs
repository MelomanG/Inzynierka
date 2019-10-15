using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class BoardGameCategory : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<BoardGame> BoardGames { get; set; } = new HashSet<BoardGame>();
    }
}