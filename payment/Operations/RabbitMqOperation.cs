using payment.Constants;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.ModelBuilders;

namespace payment.Operations
{
    public class RabbitMqOperation : RabbitMqOperationBase, IRabbitMqOperation
    {
        public RabbitMqOperation(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public void SendPaymentCreateMessage(IPayment payment)
        {
            var paymentCreatedMessage = PaymentModelBuilder.BuildCreateMessage(payment);

            SendMessage(paymentCreatedMessage, exchange: RabbitMqConstants.AmqDirectExchange, routingKey: payment.PaymentType.Code);
        }
    }
}
