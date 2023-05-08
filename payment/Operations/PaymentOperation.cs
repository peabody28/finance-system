using payment.Helpers;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.ModelBuilders;

namespace payment.Operations
{
    public class PaymentOperation : IPaymentOperation
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly IBalanceOperationTypeOperation balanceOperationTypeOperation;

        private readonly ICurrencyRateOperation currencyRateOperation;

        private readonly IRabbitMqOperation rabbitMqOperation;

        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        private readonly ILogger<PaymentOperation> logger;

        public PaymentOperation(IPaymentRepository paymentRepository, IBalanceOperationTypeOperation balanceOperationTypeOperation,
            ICurrencyRateOperation currencyRateOperation, IRabbitMqOperation rabbitMqOperation, Microsoft.Extensions.Configuration.IConfiguration configuration, ILogger<PaymentOperation> logger)
        {
            this.paymentRepository = paymentRepository;
            this.balanceOperationTypeOperation = balanceOperationTypeOperation;
            this.currencyRateOperation = currencyRateOperation;
            this.rabbitMqOperation = rabbitMqOperation;
            this.configuration = configuration;
            this.logger = logger;
        }
        
        public bool TryCreate(IWallet wallet, IBalanceOperationType balanceOperationType, decimal amount)
        {
            var payment = paymentRepository.Create(wallet, balanceOperationType, amount);
            var isPaymentCreated = payment != null;

            if (isPaymentCreated)
                SendWalletCreateMessage(payment);

            return isPaymentCreated;
        }

        public bool TryTransfer(IWallet walletFrom, IWallet walletTo, decimal amount)
        {
            var currencyRate = currencyRateOperation.Get(walletFrom.Currency.Code, walletTo.Currency.Code);
            
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

        private void SendWalletCreateMessage(IPayment payment)
        {
            logger.LogInformation("Payment created: paymentId: {id}, walletNumber: {walletNumber}, amount: {amount}, currencyCode: {currencyCode}",
                    payment.Id, payment.Wallet.Number, payment.Amount, payment.Wallet.Currency.Code);

            var paymentCreatedMessage = PaymentModelBuilder.BuildCreateMessage(payment);
            var paymentCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:PaymentCreate");

            rabbitMqOperation.SendMessage(paymentCreatedMessage, paymentCreateQueueName);
        }
    }
}
