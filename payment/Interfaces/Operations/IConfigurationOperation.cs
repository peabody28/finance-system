namespace payment.Interfaces.Operations
{
    public interface IConfigurationOperation
    {
        T? Get<T>(string key);
    }
}
