using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IBalanceOperation
    {
        decimal Get(IWallet wallet);
    }
}
