using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder
                .HasIndex(rate => new {rate.BoardGameId, rate.OwnerEmail })
                .IsUnique();

            builder
                .HasOne(rate => rate.BoardGame)
                .WithMany(bg => bg.Rates)
                .HasForeignKey(rate => rate.BoardGameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}