using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Entities
{
    [Table("configuration")]
    public class ConfigurationEntity : Interfaces.Entities.IConfiguration
    {
        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
