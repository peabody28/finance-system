using Newtonsoft.Json;

namespace payment.Models.Payment
{
    public class TransferCreateModel
    {
        [JsonProperty("walletNumberFrom")]
        public string WalletNumberFrom { get; set; }

        [JsonProperty("walletNumberTo")]
        public string WalletNumberTo { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
