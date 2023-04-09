using wallet.Interfaces.Entities;

namespace wallet.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        IWallet Create(string number, ICurrency currency, IUser user);

        IEnumerable<IWallet> Get(IUser user);

        IWallet? Get(string number, IUser? user = null);
    }
}
