using payment.Helpers;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;

namespace payment.Operations
{
    public class PaymentOperation : IPaymentOperation
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly IBalanceOperationTypeOperation balanceOperationTypeOperation;

        private readonly IWalletApiOperation walletApiOperation;

        private readonly ICurrencyRateOperation currencyRateOperation;

        public PaymentOperation(IPaymentRepository paymentRepository, IBalanceOperationTypeOperation balanceOperationTypeOperation,
            ICurrencyRateOperation currencyRateOperation, IWalletApiOperation walletApiOperation)
        {
            this.paymentRepository = paymentRepository;
            this.balanceOperationTypeOperation = balanceOperationTypeOperation;
            this.currencyRateOperation = currencyRateOperation;
            this.walletApiOperation = walletApiOperation;
        }

        public bool Transfer(IWallet walletFrom, IWallet walletTo, decimal amount)
        {
            var currencyFromCode = walletApiOperation.CurrencyCode(walletFrom.Number);
            var currencyToCode = walletApiOperation.CurrencyCode(walletTo.Number);

            var currencyRate = currencyRateOperation.Get(currencyFromCode, currencyToCode);
            
            if (!currencyRate.HasValue)
                throw new ArgumentException("Currency rate is not set");

            try
            {
                var creditAmount = AmountHelper.Compute(amount, currencyRate.Value);
                paymentRepository.BeginTransaction();
                paymentRepository.Create(walletFrom, balanceOperationTypeOperation.Debit, amount);

                paymentRepository.Create(walletTo, balanceOperationTypeOperation.Credit, creditAmount);
                paymentRepository.CommitTransaction();
                return true;
            }
            catch
            {
                paymentRepository.RollbackTransaction();
                return false;
            }
        }
    }
}
