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

        public IWallet Create(string number)
        {
            var entity = serviceProvider.GetRequiredService<IWallet>();
            entity.Id = Guid.NewGuid();
            entity.Number = number;

            var wallet = dbContext.Wallet.Add(entity as WalletEntity);
            dbContext.SaveChanges();

            return wallet.Entity;
        }

        public IWallet? Get(string number)
        {
            return dbContext.Wallet.FirstOrDefault(w => w.Number.Equals(number));
        }

        public IWallet GetOrCreate(string number)
        {
            return Get(number) ?? Create(number);
        }
    }
}
