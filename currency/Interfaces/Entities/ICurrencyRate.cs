namespace currency.Interfaces.Entities
{
    public interface ICurrencyRate
    {
        Guid Id { get; set; }

        Guid CurrencyFromFk { get; set; }
        ICurrency CurrencyFrom { get; set; }

        Guid CurrencyToFk { get; set; }
        ICurrency CurrencyTo { get; set; }

        decimal Value { get; set; }

        DateTime Date { get; set; }
    }
}
