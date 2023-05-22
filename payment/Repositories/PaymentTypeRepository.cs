using Elasticsearch.Net;
using payment.Interfaces.Entities;
using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class PaymentTypeRepository : IPaymentTypeRepository
    {
        private readonly PaymentDbContext dbContext;

        public PaymentTypeRepository(PaymentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IPaymentType? Get(string code)
        {
            return dbContext.PaymentType.FirstOrDefault(c => c.Code.Equals(code));
        }

        public IEnumerable<IPaymentType> Get()
        {
            return dbContext.PaymentType.ToList();
        }
    }
}
