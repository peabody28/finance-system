using Newtonsoft.Json;

namespace payment.worker.Models.DTO
{
    public class WalletCreateDtoModel
    {
        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
