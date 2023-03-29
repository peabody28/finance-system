using Microsoft.EntityFrameworkCore;
using wallet.Entities;

namespace wallet.Repositories
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
            
        }

        public DbSet<UserEntity> User { get; set; }

        public DbSet<WalletEntity> Wallet { get; set; }

        public DbSet<CurrencyEntity> Currency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WalletEntity>()
               .HasOne(c => c.User as UserEntity);

            modelBuilder.Entity<WalletEntity>()
              .HasOne(c => c.Currency as CurrencyEntity);
        }
    }
}
