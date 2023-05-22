using FluentValidation;
using payment.Interfaces.Entities;
using payment.Interfaces.Repositories;
using payment.Interfaces.Validations;
using payment.Models.Payment;
using Validation.Helper.Extensions;

namespace payment.Validators.Payment
{
    public class WithdrawValidator : AbstractValidator<WithdrawModel>
    {
        private readonly IWalletRepository walletRepository;

        #region [ Model Converters ]

        private IWallet? Wallet(PaymentModelBase model) => walletRepository.Get(model.WalletNumber);

        #endregion

        public WithdrawValidator(IWalletValidation walletValidation, IBalanceValidation balanceValidation, IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;

            RuleFor(model => model)
                .Custom((model, context) => context.AddFailures(nameof(model.WalletNumber), walletValidation.Validate(model.WalletNumber)))
                .Custom((model, context) => context.AddFailures(nameof(model.Amount), balanceValidation.ValidateWalletForDebit(Wallet(model), model.Amount)));
        }
    }
}
