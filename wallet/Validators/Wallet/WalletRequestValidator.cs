using FluentValidation;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;
using wallet.Models.Wallet;

namespace wallet.Validators.Wallet
{
    public class WalletRequestValidator : AbstractValidator<WalletRequestModel>
    {
        private readonly IWalletRepository walletRepository;

        public WalletRequestValidator(IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;

            RuleFor(model => model)
                .Custom(ValidateWalletNumber);
        }

        private void ValidateWalletNumber(WalletRequestModel model, ValidationContext<WalletRequestModel> context)
        {
            var wallet = walletRepository.Get(model.Number);
            if(wallet == null)
                context.AddFailure(nameof(model.Number), "Cannot find a wallet with specified number");
        }
    }
}
