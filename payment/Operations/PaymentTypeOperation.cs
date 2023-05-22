using payment.Enums;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;

namespace payment.Operations
{
    public class PaymentTypeOperation : IPaymentTypeOperation
    {
        private readonly IPaymentTypeRepository paymentTypeRepository;

        public PaymentTypeOperation(IPaymentTypeRepository paymentTypeRepository)
        {
            this.paymentTypeRepository = paymentTypeRepository;
        }

        public IPaymentType CustomPay => paymentTypeRepository.Get(PaymentType.CustomPay.ToString())!;

        public IPaymentType Transfer => paymentTypeRepository.Get(PaymentType.Transfer.ToString())!;

    }
}
