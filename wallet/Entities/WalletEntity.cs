using System.ComponentModel.DataAnnotations.Schema;
using wallet.Interfaces.Entities;

namespace wallet.Entities
{
    [Table("wallet")]
    public class WalletEntity : IWallet
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        [ForeignKey("User")]
        public Guid UserFk { get; set; }
        public IUser User { get; set; }

        [ForeignKey("Currency")]
        public Guid CurrencyFk { get; set; }
        public ICurrency Currency { get; set; }

        IUser IWallet.User
        {
            get => User;
            set
            {
                User = value as UserEntity;
            }
        }

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
