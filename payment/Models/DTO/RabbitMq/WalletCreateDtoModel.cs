using Newtonsoft.Json;

namespace payment.Models.DTO.RabbitMq
{
    public class WalletCreateDtoModel
    {
        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
