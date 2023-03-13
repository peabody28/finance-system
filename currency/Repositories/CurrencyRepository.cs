using currency.Interfaces.Entities;
using currency.Interfaces.Repositories;

namespace currency.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyDbContext dbContext;

        public CurrencyRepository(CurrencyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICurrency? Get(string code)
        {
            return dbContext.Currency.FirstOrDefault(currency => currency.Code.Equals(code));
        }
    }
}
