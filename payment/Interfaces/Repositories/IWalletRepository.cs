using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        IWallet? Get(string number);

        IWallet Create(string number, ICurrency currency);
    }
}
