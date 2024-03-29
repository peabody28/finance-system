﻿using Newtonsoft.Json;

namespace payment.Models.DTO.RabbitMq
{
    public class PaymentCreatedMessageModel
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
