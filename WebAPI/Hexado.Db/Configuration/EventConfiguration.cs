using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasOne(u => u.Owner)
                .WithMany(ov => ov.OwnedEvents)
                .HasForeignKey(e => e.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(u => u.Address)
                .WithMany(ov => ov.AddressEvents)
                .HasForeignKey(e => e.AddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(u => u.Pub)
                .WithMany(ov => ov.Events)
                .HasForeignKey(e => e.PubId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(u => u.BoardGame)
                .WithMany(ov => ov.Events)
                .HasForeignKey(e => e.BoardGameId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}