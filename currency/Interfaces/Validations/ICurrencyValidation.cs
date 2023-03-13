using validation.helper.Extensions;

namespace currency.Interfaces.Validations
{
    public interface ICurrencyValidation
    {
        ValidationResult Validate(string code);
    }
}
