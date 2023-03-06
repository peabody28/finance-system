using wallet.Interfaces.Entities;

namespace wallet.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        ICurrency? Get(string code);
    }
}
