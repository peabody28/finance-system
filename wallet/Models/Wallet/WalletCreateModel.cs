using Newtonsoft.Json;

namespace wallet.Models.Wallet
{
    public class WalletCreateModel
    {
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
