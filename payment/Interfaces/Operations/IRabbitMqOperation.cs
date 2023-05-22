using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IRabbitMqOperation
    {
        void SendPaymentCreateMessage(IPayment payment);

    }
}
