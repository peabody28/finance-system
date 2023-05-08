using payment.Interfaces.Entities;
using payment.Models.DTO.RabbitMq;
using payment.Models.Payment;

namespace payment.ModelBuilders
{
    public class PaymentModelBuilder
    {
        public static PaymentCreatedMessageModel BuildCreateMessage(IPayment payment)
        {
            return new PaymentCreatedMessageModel
            {
                Id = payment.Id,
                WalletNumber = payment.Wallet.Number,
                Amount = payment.Amount,
                BalanceOperationTypeCode = payment.BalanceOperationType.Code,
                Created = payment.Created,
            };
        }

        public static PaymentModel Build(IPayment payment)
        {
            return new PaymentModel
            {
                WalletNumber = payment.Wallet.Number,
                BalanceOperationTypeCode = payment.BalanceOperationType.Code,
                Amount = payment.Amount,
            };
        }
    }
}
