using Microsoft.EntityFrameworkCore;
using user.Entities;
using user.Interfaces.Entities;
using user.Interfaces.Repositories;

namespace user.Repositories
{
    public class UserRepository : IUserRepository
    {
        private UserDbContext DbContext { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        public UserRepository(UserDbContext userDbContext, IServiceProvider serviceProvider)
        {
            DbContext = userDbContext;
            ServiceProvider = serviceProvider;
        }

        public IUser? Create(string name, string paswordHash, IRole role)
        {
            var entity = ServiceProvider.GetRequiredService<IUser>();
            entity.Id = Guid.NewGuid();
            entity.Name = name;
            entity.PasswordHash = paswordHash;
            entity.Role = role;

            var user = DbContext.User.Add(entity as UserEntity);
            DbContext.Entry(entity.Role).State = EntityState.Unchanged;

            DbContext.SaveChanges();

            return user.Entity;
        }

        public IUser? Get(string name, string passwordHash)
        {
            return DbContext.User.Include(user => user.Role).FirstOrDefault(user => user.Name.Equals(name) && user.PasswordHash.Equals(passwordHash));
        }
    }
}
