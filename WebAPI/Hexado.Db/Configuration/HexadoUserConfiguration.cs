using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class HexadoUserConfiguration : IEntityTypeConfiguration<HexadoUser>
    {
        public void Configure(EntityTypeBuilder<HexadoUser> builder)
        {
            builder
                .HasOne(user => user.Account)
                .WithOne(account => account.HexadoUser)
                .HasForeignKey<UserAccount>(account => account.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}