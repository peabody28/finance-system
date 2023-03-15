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

        private readonly ICurrencyApiOperation currencyApiOperation;

        public PaymentOperation(IPaymentRepository paymentRepository, IBalanceOperationTypeOperation balanceOperationTypeOperation,
            ICurrencyApiOperation currencyApiOperation, IWalletApiOperation walletApiOperation)
        {
            this.paymentRepository = paymentRepository;
            this.balanceOperationTypeOperation = balanceOperationTypeOperation;
            this.currencyApiOperation = currencyApiOperation;
            this.walletApiOperation = walletApiOperation;
        }

        public bool Transfer(IWallet walletFrom, IWallet walletTo, decimal amount)
        {
            var currencyFromCode = walletApiOperation.CurrencyCode(walletFrom.Number);
            var currencyToCode = walletApiOperation.CurrencyCode(walletTo.Number);

            decimal? rate = 1;
            if(!currencyFromCode.Equals(currencyToCode))
                rate = currencyApiOperation.Rate(currencyFromCode, currencyToCode);

            if (!rate.HasValue)
                return false;

            try
            {
                var creditAmount = amount * rate.Value;
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
