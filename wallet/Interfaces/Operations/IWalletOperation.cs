using wallet.Interfaces.Entities;

namespace wallet.Interfaces.Operations
{
    public interface IWalletOperation
    {
        string GenerateNumber();

        IWallet Create(string walletNumber, ICurrency currency, IUser user);
    }
}
