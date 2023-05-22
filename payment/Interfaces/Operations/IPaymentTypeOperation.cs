using payment.Interfaces.Entities;

namespace payment.Interfaces.Operations
{
    public interface IPaymentTypeOperation
    {
        IPaymentType CustomPay { get; }

        IPaymentType Transfer { get; }
    }
}
