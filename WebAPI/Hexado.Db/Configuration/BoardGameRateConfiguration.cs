using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class BoardGameRateConfiguration : IEntityTypeConfiguration<BoardGameRate>
    {
        public void Configure(EntityTypeBuilder<BoardGameRate> builder)
        {
            builder
                .HasAlternateKey(bgr => new {bgr.BoardGameId, bgr.HexadoUserId});

            builder
                .HasOne(bgr => bgr.BoardGame)
                .WithMany(bg => bg.BoardGameRates)
                .HasForeignKey(bgr => bgr.BoardGameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(bgr => bgr.HexadoUser)
                .WithMany(hu=>hu.BoardGameRates)
                .HasForeignKey(bgr => bgr.HexadoUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}