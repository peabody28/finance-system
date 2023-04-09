using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using wallet.Interfaces.Operations;

namespace wallet.Operations
{
    public class RabbitMqOperation : IRabbitMqOperation
    {
        private readonly IModel channel;

        public RabbitMqOperation(IServiceProvider serviceProvider)
        {
            var connectionFactory = serviceProvider.GetRequiredService<ConnectionFactory>();
            var connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        /// <summary>
        /// Send message to an existing queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="queue"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        public void SendMessage<T>(T data, string queue, string exchange = "", string? routingKey = null)
        {
            var message = JsonConvert.SerializeObject(data, Formatting.Indented);

            if (string.IsNullOrWhiteSpace(routingKey))
                routingKey = queue;

            var body = Encoding.UTF8.GetBytes(message);

            var props = channel.CreateBasicProperties();
            props.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: body);
        }
    }
}
