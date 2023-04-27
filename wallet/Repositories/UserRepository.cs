using wallet.Entities;
using wallet.Interfaces.Entities;
using wallet.Interfaces.Repositories;

namespace wallet.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WalletDbContext dbContext;

        private readonly IServiceProvider serviceProvider;

        public UserRepository(WalletDbContext walletDbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = walletDbContext;
            this.serviceProvider = serviceProvider;
        }

        public IUser Create(string name)
        {
            var entity = serviceProvider.GetRequiredService<IUser>();
            entity.Id = Guid.NewGuid();
            entity.Name = name;

            var user = dbContext.User.Add(entity as UserEntity);

            dbContext.SaveChanges();

            return user.Entity;
        }

        public IUser? Get(string name)
        {
            return dbContext.User.FirstOrDefault(user => user.Name.Equals(name));
        }

        public IUser GetOrCreate(string name)
        {
            return Get(name) ?? Create(name);
        }
    }
}
