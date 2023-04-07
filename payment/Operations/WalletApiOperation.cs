using Http.Helper.Operations;
using Newtonsoft.Json;
using payment.Constants;
using payment.Interfaces.Operations;
using payment.Models.DTO.Wallet;
using System.Net;

namespace payment.Operations
{
    public class WalletApiOperation : ApiOperationBase, IWalletApiOperation
    {
        protected override string? Route => configurationOperation.Get<string>(ConfigurationConstants.WALLET_MS_ROUTE);

        public WalletApiOperation(IConfigurationOperation configurationOperation) : base(configurationOperation) { }

        /// <summary>
        /// Get currency code of wallet from external source by number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string? CurrencyCode(string number)
        {
            var requestOperation = new RequestOperation();

            var url = string.Concat(Route, number);

            var response = requestOperation.Get(url, null, AuthorizationHeaders).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;

            var walletModel = JsonConvert.DeserializeObject<WalletDtoModel>(responseContent);

            return walletModel?.CurrencyCode;
        }
    }
}
