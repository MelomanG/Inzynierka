using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class LikedPubConfiguration : IEntityTypeConfiguration<LikedPub>
    {
        public void Configure(EntityTypeBuilder<LikedPub> builder)
        {
            builder
                .HasAlternateKey(bgr => new { bgr.PubId, bgr.HexadoUserId });

            builder
                .HasOne(bgr => bgr.Pub)
                .WithMany(bg => bg.LikedPubs)
                .HasForeignKey(bgr => bgr.PubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(bgr => bgr.HexadoUser)
                .WithMany(hu => hu.LikedPubs)
                .HasForeignKey(bgr => bgr.HexadoUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}