using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            builder.ApplyConfiguration(new BoardGameConfiguration());
            builder.ApplyConfiguration(new BoardGameCategoryConfiguration());
        }

        public DbSet<HexadoUser> HexadoUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<BoardGame> BoardGames { get; set; }
        public DbSet<BoardGameCategory> BoardGamesCategories { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddBaseInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddBaseInfo()
        {
            var entries = ChangeTracker.Entries().Where(x =>
                x.Entity is IBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                var baseEntity = (IBaseEntity) entry.Entity;

                baseEntity.Modified = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                    baseEntity.Created = baseEntity.Modified;
                else if (entry.State == EntityState.Modified)
                {
                    Entry(baseEntity).Property(e => e.Created).IsModified = false;
                }
            }
        }
    }
}