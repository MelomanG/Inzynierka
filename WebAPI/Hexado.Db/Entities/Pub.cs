using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class Pub : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Address Address { get; set; }

        public string AccountId { get; set; }
        public UserAccount Account { get; set; }

        public ICollection<PubRate> PubRates { get; set; } = new HashSet<PubRate>();
        public ICollection<PubBoardGame> PubBoardGames { get; set; } = new HashSet<PubBoardGame>();
    }
}