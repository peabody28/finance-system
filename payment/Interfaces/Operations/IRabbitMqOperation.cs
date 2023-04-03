namespace payment.Interfaces.Operations
{
    public interface IRabbitMqOperation
    {
        void SendMessage<T>(T data, string queue, string exchange = "", string? routingKey = null);
    }
}
