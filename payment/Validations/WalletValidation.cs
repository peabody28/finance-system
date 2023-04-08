using payment.Interfaces.Repositories;
using payment.Interfaces.Validations;
using Validation.Helper.Extensions;

namespace payment.Validations
{
    public class WalletValidation : IWalletValidation
    {
        private readonly IWalletRepository walletRepository;

        public WalletValidation(IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;
        }

        public ValidationResult Validate(string? walletNumber, bool isRequired = true)
        {
            if (isRequired && string.IsNullOrWhiteSpace(walletNumber))
                return new ValidationResult("Wallet number required");

            return ValidationResult.Empty();
        }

        public ValidationResult CheckDuplicates(string walletNumber)
        {
            var wallet = walletRepository.Get(walletNumber);
            if (wallet != null)
                return new ValidationResult("Wallet with specified number already exists");

            return ValidationResult.Empty();
        }
    }
}
