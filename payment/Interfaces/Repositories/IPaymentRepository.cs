using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface IPaymentRepository : IRepositoryBase
    {
        IPayment Create(IWallet wallet, IBalanceOperationType balanceOperationType, decimal amount);

        IEnumerable<IPayment> Get(IWallet? wallet = null, IBalanceOperationType? balanceOperationType = null);
    }
}
