using FluentValidation;
using wallet.Interfaces.Repositories;
using wallet.Models.Wallet;

namespace wallet.Validators.Wallet
{
    public class WalletCreateValidator : AbstractValidator<WalletCreateModel>
    {
        private readonly ICurrencyRepository currencyRepository;

        public WalletCreateValidator(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;

            RuleFor(model => model)
                .Custom(ValidateCurrencyCode);
        }

        private void ValidateCurrencyCode(WalletCreateModel model, ValidationContext<WalletCreateModel> context)
        {
            var currency = currencyRepository.Get(model.CurrencyCode);
            if (currency == null)
                context.AddFailure(nameof(model.CurrencyCode), "Cannot find a currency with specified code");
        }
    }
}
