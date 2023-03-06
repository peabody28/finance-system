using Newtonsoft.Json;

namespace wallet.Models.Wallet
{
    public class WalletModel
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
