using wallet.Interfaces.Entities;

namespace wallet.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        IWallet Create(IUser user, ICurrency currency, string number);

        IEnumerable<IWallet> Get(IUser user);

        IWallet? Get(IUser user, string number);
    }
}
