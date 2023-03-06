using wallet.Interfaces.Entities;
using wallet.Interfaces.Repositories;

namespace wallet.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly WalletDbContext dbContext;

        public CurrencyRepository(WalletDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICurrency? Get(string code)
        {
            return dbContext.Currency.FirstOrDefault(currency => currency.Code.Equals(code));
        }
    }
}
