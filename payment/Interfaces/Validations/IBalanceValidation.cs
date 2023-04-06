using payment.Interfaces.Entities;
using Validation.Helper.Extensions;

namespace payment.Interfaces.Validations
{
    public interface IBalanceValidation
    {
        ValidationResult ValidateBalanceForDebit(IWallet wallet, decimal amount);
    }
}
