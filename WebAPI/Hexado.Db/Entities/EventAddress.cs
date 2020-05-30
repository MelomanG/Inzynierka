using System.Collections.Generic;

namespace Hexado.Db.Entities
{
    public class EventAddress : BaseEntity
    {
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string LocalNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public virtual ICollection<Event> AddressEvents { get; set; } = new HashSet<Event>();
    }
}