using Microsoft.EntityFrameworkCore;
using payment.Entities;
using payment.Interfaces.Entities;
using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly PaymentDbContext dbContext;

        private readonly IServiceProvider serviceProvider;

        public WalletRepository(PaymentDbContext paymentDbContext, IServiceProvider serviceProvider)
        {
            dbContext = paymentDbContext;
            this.serviceProvider = serviceProvider;
        }

        public IWallet Create(string number, ICurrency currency)
        {
            var entity = serviceProvider.GetRequiredService<IWallet>();
            entity.Id = Guid.NewGuid();
            entity.Number = number;
            entity.Currency = currency;
            
            var wallet = dbContext.Wallet.Add(entity as WalletEntity);
            dbContext.Entry(entity.Currency).State = EntityState.Unchanged;
            dbContext.SaveChanges();

            return wallet.Entity;
        }

        public IWallet? Get(string number)
        {
            return dbContext.Wallet.Include(w => w.Currency).FirstOrDefault(w => w.Number.Equals(number));
        }
    }
}
