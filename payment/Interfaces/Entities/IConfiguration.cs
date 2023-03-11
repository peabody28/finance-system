namespace payment.Interfaces.Entities
{
    public interface IConfiguration
    {
        Guid Id { get; set; }

        string Key { get; set; }

        string Value { get; set; }
    }
}
