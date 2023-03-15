using payment.Interfaces.Operations;

namespace payment.Operations
{
    public class CurrencyRateOperation : ICurrencyRateOperation
    {
        private readonly ICurrencyApiOperation currencyApiOperation;

        public CurrencyRateOperation(ICurrencyApiOperation currencyApiOperation)
        {
            this.currencyApiOperation = currencyApiOperation;
        }

        public decimal? Get(string currencyFromCode, string currencyToCode)
        {
            decimal? rate = 1;
            if (!currencyFromCode.Equals(currencyToCode))
                rate = currencyApiOperation.Rate(currencyFromCode, currencyToCode);

            return rate;
        }
    }
}
