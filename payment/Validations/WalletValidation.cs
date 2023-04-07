using payment.Interfaces.Validations;
using Validation.Helper.Extensions;

namespace payment.Validations
{
    public class WalletValidation : IWalletValidation
    {
        public ValidationResult Validate(string? walletNumber, bool isRequired = true)
        {
            if (isRequired && string.IsNullOrWhiteSpace(walletNumber))
                return new ValidationResult("Wallet number required");

            return ValidationResult.Empty();
        }
    }
}
