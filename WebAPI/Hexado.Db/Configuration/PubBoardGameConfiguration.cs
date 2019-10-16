using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class PubBoardGameConfiguration : IEntityTypeConfiguration<PubBoardGame>
    {
        public void Configure(EntityTypeBuilder<PubBoardGame> builder)
        {
            builder
                .HasAlternateKey(pr => new { pr.PubId, pr.BoardGameId });

            builder
                .HasOne(pr => pr.Pub)
                .WithMany(p => p.PubBoardGames)
                .HasForeignKey(pr => pr.PubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(pr => pr.BoardGame)
                .WithMany(p => p.PubBoardGames)
                .HasForeignKey(pr => pr.BoardGameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}