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
            //builder.ApplyConfiguration(new HexadoUserConfiguration());
        }

        public DbSet<HexadoUser> HexadoUsers { get; set; }
    }
}