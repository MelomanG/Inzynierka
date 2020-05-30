using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class Pub : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Address Address { get; set; }
        public string ImagePath { get; set; }

        public string AccountId { get; set; }
        public virtual UserAccount Account { get; set; }

        public virtual ICollection<PubRate> PubRates { get; set; } = new HashSet<PubRate>();
        public virtual ICollection<PubBoardGame> PubBoardGames { get; set; } = new HashSet<PubBoardGame>();
        public virtual ICollection<LikedPub> LikedPubs { get; set; } = new HashSet<LikedPub>();
        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}