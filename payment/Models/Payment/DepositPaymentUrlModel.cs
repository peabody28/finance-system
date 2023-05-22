using Newtonsoft.Json;

namespace payment.Models.Payment
{
    public class DepositPaymentUrlModel
    {
        [JsonProperty("paymentUrl")]
        public string? PaymentUrl { get; set; }
    }
}
