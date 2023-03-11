using payment.Enums;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;

namespace payment.Operations
{
    public class BalanceOperationTypeOperation : IBalanceOperationTypeOperation
    {
        private readonly IBalanceOperationTypeRepository balanceOperationTypeRepository;

        public BalanceOperationTypeOperation(IBalanceOperationTypeRepository balanceOperationTypeRepository)
        {
            this.balanceOperationTypeRepository = balanceOperationTypeRepository;
        }

        public IBalanceOperationType Credit => balanceOperationTypeRepository.Get(BalanceOperationType.Credit.ToString())!;

        public IBalanceOperationType Debit => balanceOperationTypeRepository.Get(BalanceOperationType.Debit.ToString())!;

    }
}
