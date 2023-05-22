using Newtonsoft.Json;

namespace payment.Models.PaymentType
{
    public class PaymentTypeModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
