using Newtonsoft.Json;

namespace payment.Models.Wallet
{
    public class WalletCreateModel
    {
        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
