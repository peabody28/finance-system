using FluentValidation;
using payment.Interfaces.Repositories;
using payment.Interfaces.Validations;
using payment.Models.Payment;
using Validation.Helper.Extensions;

namespace payment.Validators.Payment
{
    public class PaymentCreateValidator : AbstractValidator<PaymentCreateModel>
    {
        private readonly IBalanceOperationTypeRepository balanceOperationTypeRepository;

        public PaymentCreateValidator(IWalletValidation walletValidation, IBalanceOperationTypeRepository balanceOperationTypeRepository)
        {
            this.balanceOperationTypeRepository = balanceOperationTypeRepository;

            RuleFor(model => model)
                .Custom(ValidateBalanceOperationType)
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumber), walletValidation.Validate(model.WalletNumber)));
        }

        public void ValidateBalanceOperationType(PaymentCreateModel model, ValidationContext<PaymentCreateModel> context)
        {
            var balanceOperationType = balanceOperationTypeRepository.Get(model.BalanceOperationTypeCode);
            if (balanceOperationType == null)
                context.AddFailure(nameof(model.BalanceOperationTypeCode), "Cannot find a balance operation type by specified code");
        }
    }
}
