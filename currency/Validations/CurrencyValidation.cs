using currency.Interfaces.Repositories;
using currency.Interfaces.Validations;
using Validation.Helper.Extensions;

namespace currency.Validations
{
    public class CurrencyValidation : ICurrencyValidation
    {
        private readonly ICurrencyRepository currencyRepository;

        public CurrencyValidation(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public ValidationResult Validate(string code)
        {
            var currency = !string.IsNullOrWhiteSpace(code) ? currencyRepository.Get(code) : null;
            if (currency == null)
                return new ValidationResult("Currency code invalid");

            return ValidationResult.Empty();
        }
    }
}
