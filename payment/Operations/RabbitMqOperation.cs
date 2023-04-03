using Newtonsoft.Json;
using payment.Interfaces.Operations;
using RabbitMQ.Client;
using System.Text;

namespace payment.Operations
{
    public class RabbitMqOperation : IRabbitMqOperation
    {
        private readonly string hostName;

        private readonly int port;

        private readonly string userName;

        private readonly string password;

        public RabbitMqOperation(IConfiguration configuration)
        {
            hostName = configuration.GetValue<string>("RabbitMq:Host:Name");
            port = configuration.GetValue<int>("RabbitMq:Host:Port");
            userName = configuration.GetValue<string>("RabbitMq:UserName");
            password = configuration.GetValue<string>("RabbitMq:Password");
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

            var factory = new ConnectionFactory() { HostName = hostName, Port = port, UserName = userName, Password = password };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            var body = Encoding.UTF8.GetBytes(message);

            var props = channel.CreateBasicProperties();
            props.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: body);
        }
    }
}
