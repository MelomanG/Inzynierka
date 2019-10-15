using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class BoardGameCategoryConfiguration : IEntityTypeConfiguration<BoardGameCategory>
    {
        public void Configure(EntityTypeBuilder<BoardGameCategory> builder)
        {
            builder
                .HasIndex(category => category.Name)
                .IsUnique();
        }
    }
}