using http.helper.Operations;
using Microsoft.Net.Http.Headers;
using payment.Interfaces.Operations;
using System.Net;

namespace payment.Operations
{
    public class WalletApiOperation : IWalletApiOperation
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IConfiguration configuration;

        public WalletApiOperation(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
        }

        public bool IsWalletExist(string number)
        {
            var requestOperation = new RequestOperation();

            var headers = new Dictionary<string, string>();
            headers.Add(HeaderNames.Authorization, httpContextAccessor.HttpContext.Request.Headers.Authorization);

            var walletServiceUrl = configuration.GetValue<string>("Route:Wallet");

            var url = string.Concat(walletServiceUrl, number);

            var statusCode = requestOperation.Get(url, null, headers).Result;

            return statusCode.Equals(HttpStatusCode.OK);
        }
    }
}
