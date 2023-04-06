using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Validations;
using Validation.Helper.Extensions;

namespace payment.Validations
{
    public class BalanceValidation : IBalanceValidation
    {
        private readonly IBalanceOperation balanceOperation;

        public BalanceValidation(IBalanceOperation balanceOperation)
        {
            this.balanceOperation = balanceOperation;
        }

        public ValidationResult ValidateBalanceForDebit(IWallet wallet, decimal amount)
        {
            var walletBalance = balanceOperation.Balance(wallet);
            if (walletBalance < amount)
                return new ValidationResult("Insufficient funds");

            return ValidationResult.Empty();
        }
    }
}
