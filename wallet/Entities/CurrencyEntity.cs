using System.ComponentModel.DataAnnotations.Schema;
using wallet.Interfaces.Entities;

namespace wallet.Entities
{
    [Table("currency")]
    public class CurrencyEntity : ICurrency
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
    }
}
