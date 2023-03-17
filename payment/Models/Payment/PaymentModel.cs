using Newtonsoft.Json;

namespace payment.Models.Payment
{
    public class PaymentModel
    {
        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("balanceOperationTypeCode")]
        public string BalanceOperationTypeCode { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
