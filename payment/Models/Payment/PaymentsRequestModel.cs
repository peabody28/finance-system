using Newtonsoft.Json;

namespace payment.Models.Payment
{
    public class PaymentsRequestModel
    {
        [JsonProperty("walletNumber")]
        public string? WalletNumber { get; set; }
    }
}
