using currency.Interfaces.Entities;
using currency.Interfaces.Repositories;

namespace currency.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly CurrencyDbContext dbContext;

        public CurrencyRateRepository(CurrencyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICurrencyRate? Get(ICurrency currencyFrom, ICurrency currencyTo)
        {
            return dbContext.CurrencyRate.Where(rate => 
                rate.CurrencyFrom.Equals(currencyFrom) && rate.CurrencyTo.Equals(currencyTo))
                    .OrderByDescending(rate => rate.Date).FirstOrDefault();
        }
    }
}
