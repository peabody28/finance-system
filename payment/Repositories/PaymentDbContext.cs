using Microsoft.EntityFrameworkCore;
using payment.Entities;

namespace payment.Repositories
{
    public class PaymentDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public PaymentDbContext(IConfiguration config)
        {
            this.configuration = config;
        }

        public DbSet<CurrencyEntity> Currency { get; set; }

        public DbSet<WalletEntity> Wallet { get; set; }

        public DbSet<BalanceOperationTypeEntity> BalanceOperationType { get; set; }

        public DbSet<PaymentEntity> Payment { get; set; }

        public DbSet<ConfigurationEntity> Configuration { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = configuration.GetConnectionString("Payment");
            optionsBuilder.UseSqlite(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PaymentEntity>()
                .HasOne(p => p.BalanceOperationType as BalanceOperationTypeEntity);

            modelBuilder.Entity<PaymentEntity>()
                .HasOne(p => p.Wallet as WalletEntity);

            modelBuilder.Entity<WalletEntity>()
                .HasOne(p => p.Currency as CurrencyEntity);
        }
    }
}
