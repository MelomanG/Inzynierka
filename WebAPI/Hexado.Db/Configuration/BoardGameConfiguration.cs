using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class BoardGameConfiguration : IEntityTypeConfiguration<BoardGame>
    {
        public void Configure(EntityTypeBuilder<BoardGame> builder)
        {
            builder
                .HasIndex(bg => bg.Name)
                .IsUnique();

            builder
                .HasOne(bg => bg.Category)
                .WithMany(category => category.BoardGames)
                .HasForeignKey(bg => bg.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}