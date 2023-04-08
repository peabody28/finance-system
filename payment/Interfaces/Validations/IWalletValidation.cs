using Validation.Helper.Extensions;

namespace payment.Interfaces.Validations
{
    public interface IWalletValidation
    {
        ValidationResult Validate(string? walletNumber, bool isRequired = true);

        ValidationResult CheckDuplicates(string walletNumber);
    }
}
