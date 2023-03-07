using Newtonsoft.Json;

namespace payment.Models.Payment
{
    public class PaymentCreateModel
    {
        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("balanceOperationTypeCode")]
        public string BalanceOperationTypeCode { get; set; }
    }
}
