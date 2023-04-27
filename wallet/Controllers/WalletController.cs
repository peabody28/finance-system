using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wallet.Constants;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;
using wallet.Models.Wallet;

namespace wallet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly ICurrencyRepository currencyRepository;

        private readonly IWalletRepository walletRepository;

        private readonly IWalletOperation walletOperation;

        private readonly IUserOperation userOperation;

        public WalletController(ICurrencyRepository currencyRepository, IWalletRepository walletRepository, IWalletOperation walletOperation, IUserOperation userOperation)
        {
            this.currencyRepository = currencyRepository;
            this.walletRepository = walletRepository;
            this.walletOperation = walletOperation;
            this.userOperation = userOperation;
        }

        [Authorize]
        [HttpPost]
        public WalletModel Create(WalletCreateModel model)
        {
            var user = userOperation.CurrentUser;
            var currency = currencyRepository.Get(model.CurrencyCode);
            
            var walletNumber = walletOperation.GenerateNumber();

            var wallet = walletOperation.Create(walletNumber, currency, user);

            return new WalletModel { Number = wallet.Number, CurrencyCode = wallet.Currency.Code };
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<WalletModel> Get()
        {
            var user = userOperation.CurrentUser;
            var wallets = walletRepository.Get(user);

            return wallets.Select(wallet => new WalletModel { Number = wallet.Number, CurrencyCode = wallet.Currency.Code });
        }

        [Authorize(Roles = RoleConstants.MS)]
        [HttpGet]
        [Route("{number}")]
        public WalletModel Get([FromRoute] WalletRequestModel model)
        {
            var wallet = walletRepository.Get(model.Number);

            return new WalletModel { Number = wallet.Number, CurrencyCode = wallet.Currency.Code };
        }
    }
}