using System;
using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class Event: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string AddressId { get; set; }
        public EventAddress Address { get; set; }
        public string ImagePath { get; set; }
        public bool IsPublic { get; set; }

        public string OwnerId { get; set; }
        public virtual HexadoUser Owner { get; set; }
        public string PubId { get; set; }
        public virtual Pub Pub { get; set; }
        public string BoardGameId { get; set; }
        public virtual BoardGame BoardGame { get; set; }

        public virtual ICollection<ParticipantEvent> ParticipantEvents { get; set; } = new HashSet<ParticipantEvent>();
    }
}
