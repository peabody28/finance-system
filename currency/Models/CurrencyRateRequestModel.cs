using Newtonsoft.Json;

namespace currency.Models
{
    public class CurrencyRateRequestModel
    {
        [JsonProperty("currencyFromCode")]
        public string CurrencyFromCode { get; set; }

        [JsonProperty("currencyToCode")]
        public string CurrencyToCode { get; set; }
    }
}
