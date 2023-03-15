namespace payment.Interfaces.Operations
{
    public interface ICurrencyRateOperation
    {
        decimal? Get(string currencyFromCode, string currencyToCode);
    }
}
