using Newtonsoft.Json;

namespace payment.Models.DTO.Currency
{
    public class CurrencyRateDtoModel
    {
        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }
}
