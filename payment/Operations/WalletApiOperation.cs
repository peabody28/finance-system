using http.helper.Operations;
using Microsoft.Net.Http.Headers;
using payment.Interfaces.Operations;
using System.Net;

namespace payment.Operations
{
    public class WalletApiOperation : IWalletApiOperation
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IConfigurationOperation configurationOperation;

        public WalletApiOperation(IHttpContextAccessor httpContextAccessor, IConfigurationOperation configurationOperation)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configurationOperation = configurationOperation;
        }

        public bool IsWalletExist(string number)
        {
            var requestOperation = new RequestOperation();

            var headers = new Dictionary<string, string>();
            headers.Add(HeaderNames.Authorization, httpContextAccessor.HttpContext.Request.Headers.Authorization);

            var walletServiceUrl = configurationOperation.Get<string>("WALLET_MS_ROUTE");

            var url = string.Concat(walletServiceUrl, number);

            var statusCode = requestOperation.Get(url, null, headers).Result;

            return statusCode.Equals(HttpStatusCode.OK);
        }
    }
}
