using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IBalanceOperation
    {
        decimal Balance(IWallet wallet);
    }
}
