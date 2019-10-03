using Hexado.Db.Configuration;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Db
{
    public class HexadoDbContext : IdentityDbContext
    {
        public HexadoDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RefreshTokenConfiguration());
        }

        public DbSet<HexadoUser> HexadoUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}