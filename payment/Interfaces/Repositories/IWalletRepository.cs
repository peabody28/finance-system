using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        IWallet Create(string number);

        IWallet? Get(string number);

        IWallet GetOrCreate(string number);
    }
}
