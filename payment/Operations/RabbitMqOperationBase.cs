using Newtonsoft.Json;
using payment.Models;
using RabbitMQ.Client;
using System.Text;

namespace payment.Operations
{
    public abstract class RabbitMqOperationBase
    {
        private readonly IServiceProvider serviceProvider;

        public RabbitMqOperationBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected void SendMessage<T>(T data, string? queue = null, string exchange = "", string? routingKey = null)
        {
            if (string.IsNullOrWhiteSpace(routingKey))
                routingKey = queue;

            using var connection = serviceProvider.GetRequiredService<RabbitMqConnection>();

            var props = CreateDefaultProperties(connection.channel);

            connection.channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: GetBody(data));
        }

        private static byte[] GetBody<T>(T data)
        {
            var message = JsonConvert.SerializeObject(data, Formatting.Indented);
            return Encoding.UTF8.GetBytes(message);
        }

        private static IBasicProperties CreateDefaultProperties(IModel channel)
        {
            var props = channel.CreateBasicProperties();
            props.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

            return props;
        }
    }
}
