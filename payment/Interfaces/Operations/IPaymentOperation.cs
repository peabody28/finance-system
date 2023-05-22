using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IPaymentOperation
    {
        IPayment Deposit(IWallet wallet, decimal amount, out string? paymentUrl);

        IPayment Withdraw(IWallet wallet, decimal amount);
        bool TryTransfer(IWallet walletFrom, IWallet walletTo, decimal amount);
    }
}
