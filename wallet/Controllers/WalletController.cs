using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wallet.Interfaces.Entities;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;
using wallet.Models.Wallet;

namespace wallet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private IUser? CurrentUser 
        { 
            get
            {
                var name = Request.HttpContext.User?.Identity?.Name;
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                return userRepository.Get(name);
            }
        }

        private readonly IUserRepository userRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly IWalletRepository walletRepository;

        private readonly IWalletOperation walletOperation;

        public WalletController(IUserRepository userRepository, ICurrencyRepository currencyRepository, IWalletRepository walletRepository, IWalletOperation walletOperation)
        {
            this.userRepository = userRepository;
            this.currencyRepository = currencyRepository;
            this.walletRepository = walletRepository;
            this.walletOperation = walletOperation;
        }

        [Authorize]
        [HttpPost]
        public WalletModel Create(WalletCreateModel model)
        {
            var currency = currencyRepository.Get(model.CurrencyCode);
            // TODO: Fix
            if (CurrentUser == null)
                userRepository.Create(Request.HttpContext.User.Identity.Name);
            
            var walletNumber = walletOperation.GenerateNumber();

            var wallet = walletRepository.Create(CurrentUser, currency, walletNumber);

            return new WalletModel { Number = wallet.Number, CurrencyCode = wallet.Currency.Code };
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<WalletModel> Get()
        {
            var wallets = walletRepository.Get(CurrentUser);

            return wallets.Select(wallet => new WalletModel { Number = wallet.Number, CurrencyCode = wallet.Currency.Code });
        }

        [Authorize]
        [HttpGet]
        [Route("{number}")]
        public WalletModel Get([FromRoute] WalletRequestModel model)
        {
            var wallet = walletRepository.Get(CurrentUser, model.Number);

            return new WalletModel { Number = wallet.Number, CurrencyCode = wallet.Currency.Code };
        }
    }
}