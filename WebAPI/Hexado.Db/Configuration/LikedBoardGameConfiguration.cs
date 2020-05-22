using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class LikedBoardGameConfiguration : IEntityTypeConfiguration<LikedBoardGame>
    {
        public void Configure(EntityTypeBuilder<LikedBoardGame> builder)
        {
            builder
                .HasAlternateKey(bgr => new { bgr.BoardGameId, bgr.HexadoUserId });

            builder
                .HasOne(bgr => bgr.BoardGame)
                .WithMany(bg => bg.LikedBoardGames)
                .HasForeignKey(bgr => bgr.BoardGameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(bgr => bgr.HexadoUser)
                .WithMany(hu => hu.LikedBoardGames)
                .HasForeignKey(bgr => bgr.HexadoUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}