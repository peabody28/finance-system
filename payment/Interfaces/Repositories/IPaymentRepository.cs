using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        IPayment Create(IWallet wallet, IBalanceOperationType balanceOperationType, decimal amount);

        IPayment? Get(IWallet wallet, IBalanceOperationType? balanceOperationType = null);
    }
}
