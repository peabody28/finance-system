using Newtonsoft.Json;

namespace payment.Models.Payment
{
    public class PaymentModelBase
    {
        [JsonProperty("paymentTypeCode")]
        public string PaymentTypeCode { get; set; }

        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
