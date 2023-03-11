using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;

namespace payment.Operations
{
    public class ConfigurationOperation : IConfigurationOperation
    {
        private readonly IConfigurationRepository configurationRepository;

        public ConfigurationOperation(IConfigurationRepository configurationRepository) 
        {
            this.configurationRepository = configurationRepository;
        }

        public T Get<T>(string key)
        {
            var configuration = configurationRepository.Get(key);
            if (configuration == null)
                return default(T);

            return (T)Convert.ChangeType(configuration.Value, typeof(T));
        }
    }
}
