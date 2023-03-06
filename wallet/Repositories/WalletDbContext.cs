﻿using Microsoft.EntityFrameworkCore;
using wallet.Entities;

namespace wallet.Repositories
{
    public class WalletDbContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public WalletDbContext(IConfiguration config)
        {
            Configuration = config;
        }

        public DbSet<UserEntity> User { get; set; }

        public DbSet<WalletEntity> Wallet { get; set; }

        public DbSet<CurrencyEntity> Currency { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Configuration.GetConnectionString("Wallet");
            optionsBuilder.UseSqlite(connString);
        }

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
