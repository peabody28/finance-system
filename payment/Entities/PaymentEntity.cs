using payment.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Entities
{
    [Table("payment")]
    public class PaymentEntity : IPayment
    {
        public Guid Id { get; set; }

        [ForeignKey("Wallet")]
        public Guid WalletFk { get; set; }

        public IWallet Wallet { get; set; }


        [ForeignKey("BalanceOperationType")]
        public Guid BalanceOperationTypeFk { get; set; }

        public IBalanceOperationType BalanceOperationType { get; set; }

        public decimal Amount { get; set; }

        public DateTime Created { get; set; }

    }
}
