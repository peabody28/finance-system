using Newtonsoft.Json;
using payment.system.custompay.worker.Models.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace payment.system.custompay.worker
{
    public class PaymentCreateActionListener : BackgroundService
    {
        private readonly ILogger<PaymentCreateActionListener> _logger;

        private readonly IConnection _connection;

        private readonly IModel _channel;

        private readonly string _paymentCreateQueueName;

        public PaymentCreateActionListener(ILogger<PaymentCreateActionListener> logger, IConfiguration configuration)
        {
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMq:Host:Name"),
                Port = configuration.GetValue<int>("RabbitMq:Host:Port"),
                UserName = configuration.GetValue<string>("RabbitMq:UserName"),
                Password = configuration.GetValue<string>("RabbitMq:Password")
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _paymentCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:PaymentCreate");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += ProcessMessage;

            _channel.BasicConsume(_paymentCreateQueueName, false, consumer);

            await Task.Delay(3000, stoppingToken); 
        }

        private void ProcessMessage(object? ch, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var content = Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray());

            var model = JsonConvert.DeserializeObject<PaymentCreateMessageModel>(content);

            // some actions like external API call to payment system
            _logger.LogInformation("Payment with id {id} is processed", model.Id);

            _channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
        }
    }
}