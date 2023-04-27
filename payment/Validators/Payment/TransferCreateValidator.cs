using FluentValidation;
using payment.Interfaces.Entities;
using payment.Interfaces.Repositories;
using payment.Interfaces.Validations;
using payment.Models.Payment;
using Validation.Helper.Extensions;

namespace payment.Validators.Payment
{
    public class TransferCreateValidator : AbstractValidator<TransferCreateModel>
    {
        private readonly IWalletRepository walletRepository;

        private IWallet WalletFrom(TransferCreateModel model) => walletRepository.Get(model.WalletNumberFrom);

        public TransferCreateValidator(IWalletValidation walletValidation, IBalanceValidation balanceValidation, IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;

            RuleFor(model => model)
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumberFrom), walletValidation.Validate(model.WalletNumberFrom)))
                .Custom((model, context) => context.AddFailures(nameof(model.Amount), balanceValidation.ValidateWalletForDebit(WalletFrom(model), model.Amount)))
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumberTo), walletValidation.Validate(model.WalletNumberTo)));
        }
    }
}
