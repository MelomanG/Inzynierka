using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class EventAddressConfiguration : IEntityTypeConfiguration<EventAddress>
    {
        public void Configure(EntityTypeBuilder<EventAddress> builder)
        {
        }
    }
}