using Newtonsoft.Json;

namespace wallet.Models.Wallet
{
    public class WalletRequestModel
    {
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
