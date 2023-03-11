using http.helper.Operations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using payment.Interfaces.Operations;
using System.Net;

namespace payment.Operations
{
    public class WalletApiOperation : IWalletApiOperation
    {
        private readonly IConfigurationOperation configurationOperation;

        public WalletApiOperation(IConfigurationOperation configurationOperation)
        {
            this.configurationOperation = configurationOperation;
        }

        public bool IsWalletExist(string number)
        {
            var requestOperation = new RequestOperation();

            var headers = new Dictionary<string, string>();
            var clientToken = configurationOperation.Get<string>("CLIENT_TOKEN");
            headers.Add(HeaderNames.Authorization, string.Concat(JwtBearerDefaults.AuthenticationScheme, " ", clientToken));

            var walletServiceUrl = configurationOperation.Get<string>("WALLET_MS_ROUTE");

            var url = string.Concat(walletServiceUrl, number);

            var statusCode = requestOperation.Get(url, null, headers).Result;

            return statusCode.Equals(HttpStatusCode.OK);
        }
    }
}
