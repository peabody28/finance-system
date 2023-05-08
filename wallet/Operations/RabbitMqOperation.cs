using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using wallet.Interfaces.Operations;
using wallet.Models;

namespace wallet.Operations
{
    public class RabbitMqOperation : IRabbitMqOperation
    {
        private readonly IServiceProvider serviceProvider;

        public RabbitMqOperation(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
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

            using var connection = serviceProvider.GetRequiredService<RabbitMqConnection>();

            var props = CreateDefaultProperties(connection.channel);

            connection.channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: GetBody(data));
        }

        private byte[] GetBody<T>(T data)
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
