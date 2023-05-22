using payment.Interfaces.Entities;

namespace payment.Interfaces.Repositories
{
    public interface IPaymentTypeRepository
    {
        IPaymentType? Get(string code);

        IEnumerable<IPaymentType> Get();
    }
}
