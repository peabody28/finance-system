using currency.Interfaces.Entities;

namespace currency.Interfaces.Repositories
{
    public interface ICurrencyRateRepository
    {
        /// <summary>
        /// Get last currency rate
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <returns></returns>
        ICurrencyRate? Get(ICurrency currencyFrom, ICurrency currencyTo);
    }
}
