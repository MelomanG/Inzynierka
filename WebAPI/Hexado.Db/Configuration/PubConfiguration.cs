using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class PubConfiguration : IEntityTypeConfiguration<Pub>
    {
        public void Configure(EntityTypeBuilder<Pub> builder)
        {
            builder
                .HasOne(pub => pub.Account)
                .WithMany(user => user.OwnedPubs)
                .HasForeignKey(pub => pub.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}