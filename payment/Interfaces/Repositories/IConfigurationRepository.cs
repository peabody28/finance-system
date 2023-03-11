namespace payment.Interfaces.Repositories
{
    public interface IConfigurationRepository
    {
        Entities.IConfiguration? Get(string key);
    }
}
