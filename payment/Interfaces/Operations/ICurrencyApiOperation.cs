namespace payment.Interfaces.Operations
{
    public interface ICurrencyApiOperation
    {
        decimal? Rate(string currencyFromCode, string currencyToCode);
    }
}
