using currency.Entities;
using Microsoft.EntityFrameworkCore;

namespace currency.Repositories
{
    public class CurrencyDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public CurrencyDbContext(IConfiguration config)
        {
            configuration = config;
        }

        public DbSet<CurrencyRateEntity> CurrencyRate { get; set; }

        public DbSet<CurrencyEntity> Currency { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = configuration.GetConnectionString("Currency");
            optionsBuilder.UseSqlite(connString);
        }

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
