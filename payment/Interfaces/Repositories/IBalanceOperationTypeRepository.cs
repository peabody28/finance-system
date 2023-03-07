using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface IBalanceOperationTypeRepository
    {
        IBalanceOperationType? Get(string code);
    }
}
