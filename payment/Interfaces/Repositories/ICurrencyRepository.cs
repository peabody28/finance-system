using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        ICurrency? Get(string code);

        ICurrency Create(string code);

        ICurrency GetOrCreate(string code);
    }
}
