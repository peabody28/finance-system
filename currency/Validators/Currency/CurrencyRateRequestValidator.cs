using currency.Interfaces.Validations;
using currency.Models;
using FluentValidation;
using Validation.Helper.Extensions;

namespace currency.Validators.Currency
{
    public class CurrencyRateRequestValidator : AbstractValidator<CurrencyRateRequestModel>
    {
        public CurrencyRateRequestValidator(ICurrencyValidation currencyValidation)
        {
            RuleFor(model => model)
                .Custom((model, context) => context.AddFailures(nameof(model.CurrencyFromCode), currencyValidation.Validate(model.CurrencyFromCode)))
                .Custom((model, context) => context.AddFailures(nameof(model.CurrencyToCode), currencyValidation.Validate(model.CurrencyToCode)));
        }
    }
}
