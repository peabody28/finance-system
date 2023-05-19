using currency.Exceptions;
using currency.Interfaces.Repositories;
using currency.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace currency.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRepository currencyRepository;

        private readonly ICurrencyRateRepository currencyRateRepository;

        public CurrencyController(ICurrencyRepository currencyRepository, ICurrencyRateRepository currencyRateRepository)
        {
            this.currencyRepository = currencyRepository;
            this.currencyRateRepository = currencyRateRepository;
        }

        [HttpGet]
        [Route("rate")]
        public CurrencyRateModel? CurrencyRate([FromQuery] CurrencyRateRequestModel model)
        {
            var currencyFrom = currencyRepository.Get(model.CurrencyFromCode)!;
            var currencyTo = currencyRepository.Get(model.CurrencyToCode)!;
            
            var currencyRate = currencyRateRepository.Get(currencyFrom, currencyTo);
            if (currencyRate == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new CurrencyRateModel { Rate = currencyRate.Value, Date = currencyRate.Date };
        }
    }
}