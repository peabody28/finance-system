using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IPaymentOperation
    {
        bool Transfer(IWallet walletFrom, IWallet walletTo, decimal amount);
    }
}
