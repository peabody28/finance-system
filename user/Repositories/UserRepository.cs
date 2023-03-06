using Microsoft.EntityFrameworkCore;
using user.Entities;
using user.Interfaces.Entities;
using user.Interfaces.Repositories;

namespace user.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext dbContext;

        private readonly IServiceProvider serviceProvider;

        public UserRepository(UserDbContext userDbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = userDbContext;
            this.serviceProvider = serviceProvider;
        }

        public IUser? Create(string name, string paswordHash, IRole role)
        {
            var entity = serviceProvider.GetRequiredService<IUser>();
            entity.Id = Guid.NewGuid();
            entity.Name = name;
            entity.PasswordHash = paswordHash;
            entity.Role = role;

            var user = dbContext.User.Add(entity as UserEntity);
            dbContext.Entry(entity.Role).State = EntityState.Unchanged;

            dbContext.SaveChanges();

            return user.Entity;
        }

        public IUser? Get(string name, string passwordHash)
        {
            return dbContext.User.Include(user => user.Role).FirstOrDefault(user => user.Name.Equals(name) && user.PasswordHash.Equals(passwordHash));
        }

        public IUser? Get(string name)
        {
            return dbContext.User.Include(user => user.Role).FirstOrDefault(user => user.Name.Equals(name));
        }
    }
}
