using payment.Interfaces.Operations;
using payment.Interfaces.Validations;
using Validation.Helper.Extensions;

namespace payment.Validations
{
    public class WalletValidation : IWalletValidation
    {
        private readonly IWalletApiOperation walletApiOperation;

        public WalletValidation(IWalletApiOperation walletApiOperation) 
        {
            this.walletApiOperation = walletApiOperation;
        }

        public ValidationResult Validate(string? walletNumber, bool isRequired = true)
        {
            if (isRequired && string.IsNullOrWhiteSpace(walletNumber))
                return new ValidationResult("Wallet number required");

            if (!string.IsNullOrWhiteSpace(walletNumber) && !walletApiOperation.IsWalletExist(walletNumber))
                return new ValidationResult("Wallet number does not exists");

            return ValidationResult.Empty();
        }
    }
}
