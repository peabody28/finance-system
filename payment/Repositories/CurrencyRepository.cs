using payment.Entities;
using payment.Interfaces.Entities;
using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class CurrencyRepository : Repository, ICurrencyRepository
    {
        private readonly IServiceProvider serviceProvider;

        private readonly PaymentDbContext dbContext;

        public CurrencyRepository(IServiceProvider serviceProvider, PaymentDbContext dbContext) : base(dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.dbContext = dbContext;
        }

        public ICurrency? Get(string code)
        {
            return dbContext.Currency.FirstOrDefault(c => c.Code.Equals(code));
        }

        public ICurrency Create(string code)
        {
            var entity = serviceProvider.GetRequiredService<ICurrency>();
            entity.Id = Guid.NewGuid();
            entity.Code = code;

            var currency = dbContext.Currency.Add(entity as CurrencyEntity);
            dbContext.SaveChanges();

            return currency.Entity;
        }

        public ICurrency GetOrCreate(string code)
        {
            return Get(code) ?? Create(code);
        }
    }
}
