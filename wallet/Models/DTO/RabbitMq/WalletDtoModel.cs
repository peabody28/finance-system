using Newtonsoft.Json;

namespace wallet.Models.DTO.RabbitMq
{
    public class WalletDtoModel
    {
        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
