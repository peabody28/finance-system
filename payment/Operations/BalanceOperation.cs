using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;

namespace payment.Operations
{
    public class BalanceOperation : IBalanceOperation
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly IBalanceOperationTypeOperation balanceOperationTypeOperation;

        public BalanceOperation(IPaymentRepository paymentRepository, IBalanceOperationTypeOperation balanceOperationTypeOperation)
        {
            this.paymentRepository = paymentRepository;
            this.balanceOperationTypeOperation = balanceOperationTypeOperation;
        }

        public decimal Get(IWallet wallet)
        {
            var payments = paymentRepository.Get(wallet);

            Func<decimal, IPayment, decimal> updateBalance = (currentBalance, payment) => payment.BalanceOperationType.Equals(balanceOperationTypeOperation.Debit) 
                    ? Decimal.Subtract(currentBalance, payment.Amount) 
                    : Decimal.Add(currentBalance, payment.Amount);

            decimal balance = 0m;

            foreach(var payment in payments)
                balance = updateBalance(balance, payment);

            return balance;
        }
    }
}
