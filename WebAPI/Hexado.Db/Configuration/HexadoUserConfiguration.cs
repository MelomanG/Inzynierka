using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class HexadoUserConfiguration : IEntityTypeConfiguration<HexadoUser>
    {
        public void Configure(EntityTypeBuilder<HexadoUser> builder)
        {
        }
    }
}