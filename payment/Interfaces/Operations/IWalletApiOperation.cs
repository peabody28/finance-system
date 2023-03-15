namespace payment.Interfaces.Operations
{
    public interface IWalletApiOperation
    {
        bool IsWalletExist(string number);

        string? CurrencyCode(string number);
    }
}
