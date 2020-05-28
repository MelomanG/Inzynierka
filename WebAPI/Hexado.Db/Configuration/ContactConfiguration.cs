using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder
                .HasAlternateKey(pr => new { pr.HexadoUserId, pr.ContactHexadoUserId });

            builder
                .HasOne(pr => pr.ContactHexadoUser)
                .WithMany()
                .HasForeignKey(pr => pr.ContactHexadoUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(pr => pr.HexadoUser)
                .WithMany(p => p.Contacts)
                .HasForeignKey(pr => pr.HexadoUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}