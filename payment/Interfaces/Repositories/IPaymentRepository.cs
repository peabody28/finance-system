using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface IPaymentRepository : IRepository
    {
        IPayment Create(IWallet wallet, IPaymentType paymentType, IBalanceOperationType balanceOperationType, decimal amount);

        IEnumerable<IPayment> Get(IWallet? wallet = null, IBalanceOperationType? balanceOperationType = null);
    }
}
