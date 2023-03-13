using currency.Interfaces.Entities;

namespace currency.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        ICurrency? Get(string code);
    }
}
