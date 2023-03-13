using currency.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace currency.Entities
{
    [Table("currencyRate")]
    public class CurrencyRateEntity : ICurrencyRate
    {
        public Guid Id { get; set; }

        [ForeignKey("CurrencyFrom")]
        public Guid CurrencyFromFk { get; set; }
        public ICurrency CurrencyFrom { get; set; }

        [ForeignKey("CurrencyTo")]
        public Guid CurrencyToFk { get; set; }
        public ICurrency CurrencyTo { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }
    }
}
