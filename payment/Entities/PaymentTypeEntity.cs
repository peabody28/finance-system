using payment.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Entities
{
    [Table("paymentType")]
    public class PaymentTypeEntity : IPaymentType
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
    }
}
