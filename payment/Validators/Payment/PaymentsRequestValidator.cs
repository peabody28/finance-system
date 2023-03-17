using FluentValidation;
using payment.Interfaces.Validations;
using payment.Models.Payment;
using Validation.Helper.Extensions;

namespace payment.Validators.Payment
{
    public class PaymentsRequestValidator : AbstractValidator<PaymentsRequestModel>
    {
        public PaymentsRequestValidator(IWalletValidation walletValidation)
        {
            RuleFor(model => model)
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumber), walletValidation.Validate(model.WalletNumber, false)));
        }
    }
}
