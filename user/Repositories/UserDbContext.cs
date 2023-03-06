using Microsoft.EntityFrameworkCore;
using user.Entities;

namespace user.Repositories
{
    public class UserDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public UserDbContext(IConfiguration config)
        {
            this.configuration = config;
        }

        public DbSet<UserEntity> User { get; set; }

        public DbSet<RoleEntity> Role { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = configuration.GetConnectionString("User");
            optionsBuilder.UseSqlite(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
               .HasOne(c => c.Role as RoleEntity);
        }
    }
}
