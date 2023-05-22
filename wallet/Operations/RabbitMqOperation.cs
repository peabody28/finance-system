using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using wallet.Interfaces.Operations;

namespace wallet.Operations
{
    public class RabbitMqOperation : IRabbitMqOperation
    {
        private readonly ConnectionFactory connectionFactory;

        public RabbitMqOperation(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
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
            if (string.IsNullOrWhiteSpace(routingKey))
                routingKey = queue;

            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var props = CreateDefaultProperties(channel);

            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: GetBody(data));
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
