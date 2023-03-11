using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IBalanceOperationTypeOperation
    {
        IBalanceOperationType Credit { get; }

        IBalanceOperationType Debit { get; } 
    }
}
