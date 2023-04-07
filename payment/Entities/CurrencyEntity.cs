using payment.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Entities
{
    [Table("currency")]
    public class CurrencyEntity : ICurrency
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
    }
}
