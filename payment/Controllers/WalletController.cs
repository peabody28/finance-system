using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payment.Constants;
using payment.Interfaces.Repositories;
using payment.Models.Wallet;
using System.Net;

namespace payment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository walletRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly ILogger<WalletController> logger;

        public WalletController(IWalletRepository walletRepository, ICurrencyRepository currencyRepository, ILogger<WalletController> logger)
        {
            this.walletRepository = walletRepository;
            this.currencyRepository = currencyRepository;
            this.logger = logger;
        }

        [Authorize(Roles = RoleConstants.MS)]
        [HttpPost]
        public HttpResponseMessage Create(WalletCreateModel model)
        {
            var currency = currencyRepository.GetOrCreate(model.CurrencyCode);

            var wallet = walletRepository.Create(model.WalletNumber, currency);

            if (wallet != null)
                logger.LogInformation("Wallet created: walletNumber: {walletNumber}, currencyCode: {currencyCode}", wallet.Number, currency.Code);

            return new HttpResponseMessage(wallet != null ? HttpStatusCode.Created : HttpStatusCode.InternalServerError);
        }
    }
}
