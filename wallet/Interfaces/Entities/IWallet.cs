namespace wallet.Interfaces.Entities
{
    public interface IWallet
    {
        Guid Id { get; set; }

        string Number { get; set; }

        Guid UserFk { get; set; }

        IUser User { get; set; }

        Guid CurrencyFk { get; set; }

        ICurrency Currency { get; set; }
    }
}
