using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IPaymentOperation
    {
        IPayment Deposit(IWallet wallet, IPaymentType paymentType, decimal amount, out string? paymentUrl);

        IPayment Withdraw(IWallet wallet, IPaymentType paymentType, decimal amount);

        bool TryTransfer(IWallet walletFrom, IWallet walletTo, decimal amount);
    }
}
