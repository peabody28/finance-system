using FluentValidation;
using payment.Interfaces.Validations;
using payment.Models.Payment;
using validation.helper.Extensions;

namespace payment.Validators.Payment
{
    public class TransferCreateValidator : AbstractValidator<TransferCreateModel>
    {
        public TransferCreateValidator(IWalletValidation walletValidation)
        {
            RuleFor(model => model)
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumberFrom), walletValidation.Validate(model.WalletNumberFrom)))
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumberTo), walletValidation.Validate(model.WalletNumberTo)));
        }
    }
}
