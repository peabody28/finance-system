using RabbitMQ.Client;

namespace payment.Models
{
    class RabbitMqConnection : IDisposable
    {
        private readonly IConnection connection;

        public readonly IModel channel;

        public RabbitMqConnection(ConnectionFactory connectionFactory)
        {
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
