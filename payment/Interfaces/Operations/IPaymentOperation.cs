using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IPaymentOperation
    {
        bool TryCreate(IWallet wallet, IBalanceOperationType balanceOperationType, decimal amount);

        bool TryTransfer(IWallet walletFrom, IWallet walletTo, decimal amount);
    }
}
