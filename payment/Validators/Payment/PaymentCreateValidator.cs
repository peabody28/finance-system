using FluentValidation;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.Models.Payment;

namespace payment.Validators.Payment
{
    public class PaymentCreateValidator : AbstractValidator<PaymentCreateModel>
    {
        private readonly IWalletApiOperation walletApiOperation;

        private readonly IBalanceOperationTypeRepository balanceOperationTypeRepository;

        public PaymentCreateValidator(IWalletApiOperation walletApiOperation, IBalanceOperationTypeRepository balanceOperationTypeRepository)
        {
            this.walletApiOperation = walletApiOperation;
            this.balanceOperationTypeRepository = balanceOperationTypeRepository;

            RuleFor(model => model)
                .Custom(ValidateBalanceOperationType)
                .Custom(ValidateWalletNumber);
        }

        public void ValidateWalletNumber(PaymentCreateModel model, ValidationContext<PaymentCreateModel> context)
        {
            if (!walletApiOperation.IsWalletExist(model.WalletNumber))
                context.AddFailure(nameof(model.WalletNumber), "Wallet number does not exists");
        }

        public void ValidateBalanceOperationType(PaymentCreateModel model, ValidationContext<PaymentCreateModel> context)
        {
            var balanceOperationType = balanceOperationTypeRepository.Get(model.BalanceOperationTypeCode);
            if (balanceOperationType == null)
                context.AddFailure(nameof(model.BalanceOperationTypeCode), "Cannot find a balance operation type by specified code");
        }
    }
}
