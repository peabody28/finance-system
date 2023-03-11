using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly PaymentDbContext dbContext;

        public ConfigurationRepository(PaymentDbContext paymentDbContext)
        {
            dbContext = paymentDbContext;
        }

        public Interfaces.Entities.IConfiguration? Get(string key)
        {
            return dbContext.Configuration.FirstOrDefault(c => c.Key.Equals(key));
        }
    }
}
