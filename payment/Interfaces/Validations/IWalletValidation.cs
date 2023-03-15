using Validation.Helper.Extensions;

namespace payment.Interfaces.Validations
{
    public interface IWalletValidation
    {
        ValidationResult Validate(string walletNumber);
    }
}
