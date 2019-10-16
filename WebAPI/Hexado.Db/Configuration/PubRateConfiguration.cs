using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class PubRateConfiguration : IEntityTypeConfiguration<PubRate>
    {
        public void Configure(EntityTypeBuilder<PubRate> builder)
        {
            builder
                .HasAlternateKey(pr => new {pr.PubId, pr.HexadoUserId});

            builder
                .HasOne(pr => pr.Pub)
                .WithMany(p => p.PubRates)
                .HasForeignKey(pr => pr.PubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(pr => pr.HexadoUser)
                .WithMany(p => p.PubRates)
                .HasForeignKey(pr => pr.HexadoUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}