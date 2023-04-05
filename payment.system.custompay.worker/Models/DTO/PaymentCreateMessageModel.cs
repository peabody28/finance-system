using Newtonsoft.Json;

namespace payment.system.custompay.worker.Models.DTO
{
    public class PaymentCreateMessageModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("walletNumber")]
        public string WalletNumber { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("balanceOperationTypeCode")]
        public string BalanceOperationTypeCode { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }
    }
}
