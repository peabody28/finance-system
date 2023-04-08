using FluentValidation;
using payment.Interfaces.Validations;
using payment.Models.Wallet;
using Validation.Helper.Extensions;

namespace payment.Validators.Wallet
{
    public class WalletCreateValidator : AbstractValidator<WalletCreateModel>
    {
        public WalletCreateValidator(IWalletValidation walletValidation) 
        {
            RuleFor(model => model)
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumber), walletValidation.Validate(model.WalletNumber)))
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumber), walletValidation.CheckDuplicates(model.WalletNumber)));

            RuleFor(model => model.CurrencyCode)
                .NotEmpty();
        }
    }
}
