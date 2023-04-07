using FluentValidation;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.Interfaces.Validations;
using payment.Models.Payment;
using Validation.Helper.Extensions;

namespace payment.Validators.Payment
{
    public class PaymentCreateValidator : AbstractValidator<PaymentCreateModel>
    {
        private readonly IBalanceOperationTypeRepository balanceOperationTypeRepository;

        private readonly IWalletRepository walletRepository;

        #region [ Model Converters ]

        private IWallet? Wallet(PaymentCreateModel model) => walletRepository.Get(model.WalletNumber);

        #endregion

        public PaymentCreateValidator(IWalletValidation walletValidation, IBalanceValidation balanceValidation, IBalanceOperationTypeOperation balanceOperationTypeOperation,
            IBalanceOperationTypeRepository balanceOperationTypeRepository, IWalletRepository walletRepository)
        {
            this.balanceOperationTypeRepository = balanceOperationTypeRepository;
            this.walletRepository = walletRepository;

            RuleFor(model => model)
                .Custom(ValidateBalanceOperationType)
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumber), walletValidation.Validate(model.WalletNumber)))
                .Custom((model, context) => context.AddFailures(nameof(model.Amount), balanceValidation.ValidateBalanceForDebit(Wallet(model), model.Amount)))
                    .When(model => model.BalanceOperationTypeCode.Equals(balanceOperationTypeOperation.Debit.Code), ApplyConditionTo.CurrentValidator);
        }

        public void ValidateBalanceOperationType(PaymentCreateModel model, ValidationContext<PaymentCreateModel> context)
        {
            var balanceOperationType = balanceOperationTypeRepository.Get(model.BalanceOperationTypeCode);
            if (balanceOperationType == null)
                context.AddFailure(nameof(model.BalanceOperationTypeCode), "Cannot find a balance operation type by specified code");
        }
    }
}
