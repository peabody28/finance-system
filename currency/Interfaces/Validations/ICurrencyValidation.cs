using Validation.Helper.Extensions;

namespace currency.Interfaces.Validations
{
    public interface ICurrencyValidation
    {
        ValidationResult Validate(string code);
    }
}
