using currency.Entities;
using Microsoft.EntityFrameworkCore;

namespace currency.Repositories
{
    public class CurrencyDbContext : DbContext
    {
        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
        {
            
        }

        public DbSet<CurrencyRateEntity> CurrencyRate { get; set; }

        public DbSet<CurrencyEntity> Currency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CurrencyRateEntity>()
                .HasOne(c => c.CurrencyFrom as CurrencyEntity);

            modelBuilder.Entity<CurrencyRateEntity>()
                .HasOne(c => c.CurrencyTo as CurrencyEntity);
        }
    }
}
