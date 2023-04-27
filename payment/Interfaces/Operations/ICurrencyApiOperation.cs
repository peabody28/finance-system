namespace payment.Interfaces.Operations
{
    public interface ICurrencyApiOperation
    {
        decimal? GetRate(string currencyFromCode, string currencyToCode);
    }
}
