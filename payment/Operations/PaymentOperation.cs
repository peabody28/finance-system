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

        private readonly IPaymentTypeOperation paymentTypeOperation;

        private readonly ICurrencyRateOperation currencyRateOperation;

        private readonly IRabbitMqOperation rabbitMqOperation;

        private readonly ILogger<PaymentOperation> logger;

        public PaymentOperation(IPaymentRepository paymentRepository, IBalanceOperationTypeOperation balanceOperationTypeOperation, IPaymentTypeOperation paymentTypeOperation,
            ICurrencyRateOperation currencyRateOperation, IRabbitMqOperation rabbitMqOperation, ILogger<PaymentOperation> logger)
        {
            this.paymentRepository = paymentRepository;
            this.balanceOperationTypeOperation = balanceOperationTypeOperation;
            this.paymentTypeOperation = paymentTypeOperation;
            this.currencyRateOperation = currencyRateOperation;
            this.rabbitMqOperation = rabbitMqOperation;
            this.logger = logger;
        }

        public IPayment Deposit(IWallet wallet, IPaymentType paymentType, decimal amount, out string? paymentUrl)
        {
            var payment = paymentRepository.Create(wallet, paymentType, balanceOperationTypeOperation.Credit, amount);

            paymentUrl = null;

            return payment;
        }

        public IPayment Withdraw(IWallet wallet, IPaymentType paymentType, decimal amount)
        {
            var payment = paymentRepository.Create(wallet, paymentType, balanceOperationTypeOperation.Debit, amount);

            PaymentCreatedLog(payment);

            rabbitMqOperation.SendPaymentCreateMessage(payment);

            return payment;
        }

        public bool TryTransfer(IWallet walletFrom, IWallet walletTo, decimal amount)
        {
            var currencyRate = currencyRateOperation.Get(walletFrom.Currency.Code, walletTo.Currency.Code);
            
            if (!currencyRate.HasValue)
                throw new ArgumentException("Currency rate is not set");

            try
            {
                var paymentType = paymentTypeOperation.Transfer;
                var creditAmount = AmountHelper.Compute(amount, currencyRate.Value);
                paymentRepository.BeginTransaction();
                paymentRepository.Create(walletFrom, paymentType, balanceOperationTypeOperation.Debit, amount);

                paymentRepository.Create(walletTo, paymentType, balanceOperationTypeOperation.Credit, creditAmount);
                paymentRepository.CommitTransaction();
                return true;
            }
            catch
            {
                paymentRepository.RollbackTransaction();
                return false;
            }
        }

        private void PaymentCreatedLog(IPayment payment)
        {
            logger.LogInformation("Payment created: paymentId: {id}, walletNumber: {walletNumber}, balanceOperationType: {balanceOperationType} amount: {amount}, currencyCode: {currencyCode}",
                    payment.Id, payment.Wallet.Number, payment.BalanceOperationType.Code, payment.Amount, payment.Wallet.Currency.Code);
        }
    }
}
