using Newtonsoft.Json;

namespace payment.Models.DTO.Wallet
{
    public class WalletDtoModel
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
