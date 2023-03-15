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
        private readonly IUserRepository userRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly IWalletRepository walletRepository;

        private readonly IWalletOperation walletOperation;

        private readonly IUserOperation userOperation;

        public WalletController(IUserRepository userRepository, ICurrencyRepository currencyRepository, IWalletRepository walletRepository, IWalletOperation walletOperation, IUserOperation userOperation)
        {
            this.userRepository = userRepository;
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
            // TODO: Fix
            if (user == null)
                userRepository.Create(Request.HttpContext.User.Identity.Name);
            
            var walletNumber = walletOperation.GenerateNumber();

            var wallet = walletRepository.Create(user, currency, walletNumber);

            return new WalletModel { Number = wallet.Number, CurrencyCode = wallet.Currency.Code };
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<WalletModel> Get()
        {
            var wallets = walletRepository.Get(userOperation.CurrentUser);

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