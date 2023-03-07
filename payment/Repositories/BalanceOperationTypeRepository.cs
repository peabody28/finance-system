using payment.Interfaces.Entities;
using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class BalanceOperationTypeRepository : IBalanceOperationTypeRepository
    {
        private readonly PaymentDbContext dbContext;

        public BalanceOperationTypeRepository(PaymentDbContext paymentDbContext)
        {
            dbContext = paymentDbContext;
        }

        public IBalanceOperationType? Get(string code)
        {
            return dbContext.BalanceOperationType.FirstOrDefault(b => b.Code.Equals(code));
        }
    }
}
