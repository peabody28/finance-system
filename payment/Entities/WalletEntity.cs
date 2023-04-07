using payment.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Entities
{
    [Table("wallet")]
    public class WalletEntity : IWallet
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        [ForeignKey("Currency")]
        public Guid CurrencyFk { get; set; }
        public ICurrency Currency { get; set; }

        ICurrency IWallet.Currency
        {
            get => Currency;
            set
            {
                Currency = value as CurrencyEntity;
            }
        }

    }
}
