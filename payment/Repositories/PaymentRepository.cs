using Microsoft.EntityFrameworkCore;
using payment.Entities;
using payment.Interfaces.Entities;
using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class PaymentRepository : RepositoryBase, IPaymentRepository
    {
        private readonly PaymentDbContext dbContext;

        private readonly IServiceProvider serviceProvider;

        public PaymentRepository(PaymentDbContext paymentDbContext, IServiceProvider serviceProvider) : base(paymentDbContext)
        {
            this.dbContext = paymentDbContext;
            this.serviceProvider = serviceProvider;
        }

        public IPayment Create(IWallet wallet, IBalanceOperationType balanceOperationType, decimal amount)
        {
            var entity = serviceProvider.GetRequiredService<IPayment>();
            entity.Id = Guid.NewGuid();
            entity.Wallet = wallet;
            entity.BalanceOperationType = balanceOperationType;
            entity.Amount = amount;
            entity.Created = DateTime.UtcNow;

            var payment = dbContext.Payment.Add(entity as PaymentEntity);
            dbContext.Entry(entity.Wallet).State = EntityState.Unchanged;
            dbContext.Entry(entity.BalanceOperationType).State = EntityState.Unchanged;
            dbContext.SaveChanges();

            return payment.Entity;
        }

        public IEnumerable<IPayment> Get(IWallet wallet, IBalanceOperationType? balanceOperationType = null)
        {
            return dbContext.Payment.Include(p => p.BalanceOperationType).Include(p => p.Wallet).Where(p => p.Wallet.Equals(wallet) &&
                (balanceOperationType == null || p.BalanceOperationType.Equals(balanceOperationType))).ToList();
        }
    }
}
