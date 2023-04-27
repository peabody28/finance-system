using Http.Helper.Operations;
using Newtonsoft.Json;
using payment.Constants;
using payment.Interfaces.Operations;
using payment.Models.DTO.Currency;

namespace payment.Operations
{
    public class CurrencyApiOperation : ApiOperation, ICurrencyApiOperation
    {
        protected override string? Route => configurationOperation.Get<string>(ConfigurationConstants.CURRENCY_MS_ROUTE);

        public CurrencyApiOperation(IConfigurationOperation configurationOperation) : base(configurationOperation) { }

        public decimal? GetRate(string currencyFromCode, string currencyToCode)
        {
            var requestOperation = new RequestOperation();

            var data = new Dictionary<string, string>();
            data.Add("currencyFromCode", currencyFromCode);
            data.Add("currencyToCode", currencyToCode);

            var url = string.Concat(Route, RouteConstants.CurrencyRateRoutePostfix);

            var response = requestOperation.Get(url, data, DefaultHeaders).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;

            var currencyRateModel = JsonConvert.DeserializeObject<CurrencyRateDtoModel>(responseContent);

            return currencyRateModel?.Rate;
        }
    }
}
