using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class ParticipantEventConfiguration : IEntityTypeConfiguration<ParticipantEvent>
    {
        public void Configure(EntityTypeBuilder<ParticipantEvent> builder)
        {
            builder
                .HasAlternateKey(pe => new { pe.ParticipantId, pe.EventId });

            builder
                .HasOne(pe => pe.Participant)
                .WithMany(p => p.ParticipantEvents)
                .HasForeignKey(bgr => bgr.ParticipantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(pe => pe.Event)
                .WithMany(p => p.ParticipantEvents)
                .HasForeignKey(bgr => bgr.EventId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}