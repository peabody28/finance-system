using FluentValidation;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;
using wallet.Models.Wallet;

namespace wallet.Validators.Wallet
{
    public class WalletRequestValidator : AbstractValidator<WalletRequestModel>
    {
        private readonly IWalletRepository walletRepository;

        private readonly IUserOperation userOperation;

        public WalletRequestValidator(IWalletRepository walletRepository, IUserOperation userOperation)
        {
            this.walletRepository = walletRepository;
            this.userOperation = userOperation;

            RuleFor(model => model)
                .Custom(ValidateWalletNumber);
        }

        private void ValidateWalletNumber(WalletRequestModel model, ValidationContext<WalletRequestModel> context)
        {
            var wallet = walletRepository.Get(userOperation.CurrentUser, model.Number);
            if(wallet == null)
                context.AddFailure(nameof(model.Number), "Cannot find a wallet with specified number");
        }
    }
}
