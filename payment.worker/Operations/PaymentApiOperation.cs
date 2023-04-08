using Http.Helper.Operations;
using Microsoft.Net.Http.Headers;
using payment.worker.Constants;
using payment.worker.Models.DTO;

namespace payment.worker.Operations
{
    public class PaymentApiOperation
    {
        private readonly string walletEndpoint;

        private readonly string accessToken;

        private readonly ILogger<PaymentApiOperation> logger;

        public PaymentApiOperation(IConfiguration configuration, ILogger<PaymentApiOperation> logger)
        {
            walletEndpoint = configuration.GetValue<string>("Endpoint:Wallet");
            accessToken = configuration.GetValue<string>("AccessToken");
            this.logger = logger;
        }

        public async Task<bool> TryCreateWallet(WalletCreateDtoModel model)
        {
            var requestOperation = new RequestOperation();

            var headers = new Dictionary<string, string>();
            headers.Add(HeaderNames.Authorization, string.Concat(AuthConstants.AuthenticationScheme, " ", accessToken));

            try
            {
                var message = await requestOperation.Post(walletEndpoint, model, headers);
                return message.IsSuccessStatusCode;
            }
            catch
            {
                logger.LogError("Wallet create ({number}) request failed", model.WalletNumber);
                return false;
            }
        }
    }
}
