using Newtonsoft.Json;

namespace currency.Models
{
    public class CurrencyRateModel
    {
        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
