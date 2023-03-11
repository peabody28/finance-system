using Microsoft.EntityFrameworkCore;
using wallet.Entities;
using wallet.Interfaces.Entities;
using wallet.Interfaces.Repositories;

namespace wallet.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext dbContext;

        private readonly IServiceProvider serviceProvider;

        public WalletRepository(WalletDbContext walletDbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = walletDbContext;
            this.serviceProvider = serviceProvider;
        }

        public IWallet Create(IUser user, ICurrency currency, string number)
        {
            var entity = serviceProvider.GetRequiredService<IWallet>();
            entity.Id = Guid.NewGuid();
            entity.User = user;
            entity.Currency = currency;
            entity.Number = number;

            var wallet = dbContext.Wallet.Add(entity as WalletEntity);
            dbContext.Entry(entity.User).State = EntityState.Unchanged;
            dbContext.Entry(entity.Currency).State = EntityState.Unchanged;

            dbContext.SaveChanges();

            return wallet.Entity;
        }

        public IEnumerable<IWallet> Get(IUser user)
        {
            return dbContext.Wallet.Include(w => w.User).Include(w => w.Currency).Where(w => w.User.Equals(user)).ToList();
        }

        public IWallet? Get(string number)
        {
            return dbContext.Wallet.Include(w => w.User).Include(w => w.Currency).FirstOrDefault(w => w.Number.Equals(number));
        }

        public IWallet? Get(IUser user, string number)
        {
            return dbContext.Wallet.Include(w => w.User).Include(w => w.Currency)
                .FirstOrDefault(w => w.User.Equals(user) && w.Number.Equals(number));
        }
    }
}
