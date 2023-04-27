using Microsoft.Extensions.Caching.Memory;
using payment.Constants;
using payment.Interfaces.Operations;

namespace payment.Operations
{
    public class CurrencyRateOperation : ICurrencyRateOperation
    {
        private readonly ICurrencyApiOperation currencyApiOperation;

        private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public CurrencyRateOperation(ICurrencyApiOperation currencyApiOperation)
        {
            this.currencyApiOperation = currencyApiOperation;
        }

        public decimal? Get(string currencyFromCode, string currencyToCode)
        {
            decimal? rate = 1;

            var cacheKey = string.Format(CurrencyConstants.CurrencyRateCacheKeyTemplate, currencyFromCode, currencyToCode);
            
            if(!currencyFromCode.Equals(currencyToCode) && !_cache.TryGetValue(cacheKey, out rate))
            {
                rate = currencyApiOperation.GetRate(currencyFromCode, currencyToCode);
                _cache.Set(cacheKey, rate, TimeSpan.FromDays(1));
            }

            return rate;
        }
    }
}
