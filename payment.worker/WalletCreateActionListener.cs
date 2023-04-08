using Newtonsoft.Json;
using payment.worker.Models.DTO;
using payment.worker.Operations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace payment.worker
{
    public class WalletCreateActionListener : BackgroundService
    {
        private readonly ILogger<WalletCreateActionListener> _logger;

        private readonly PaymentApiOperation paymentApiOperation;

        private readonly IConnection _connection;

        private readonly IModel _channel;

        private readonly string _walletCreateQueueName;

        public WalletCreateActionListener(ILogger<WalletCreateActionListener> logger, IConfiguration configuration, PaymentApiOperation paymentApiOperation)
        {
            _logger = logger;
            this.paymentApiOperation = paymentApiOperation;

            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMq:Host:Name"),
                Port = configuration.GetValue<int>("RabbitMq:Host:Port"),
                UserName = configuration.GetValue<string>("RabbitMq:UserName"),
                Password = configuration.GetValue<string>("RabbitMq:Password")
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _walletCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:WalletCreate");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += ProcessMessage;

            _channel.BasicConsume(_walletCreateQueueName, false, consumer);

            await Task.Delay(3000, stoppingToken);
        }

        private void ProcessMessage(object? ch, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var content = Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray());

            var model = JsonConvert.DeserializeObject<WalletCreateDtoModel>(content);

            var status = paymentApiOperation.TryCreateWallet(model).Result;

            if(status)
            {
                _logger.LogInformation("Wallet ({number}) create message procceed", model.WalletNumber);
                _channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            }
            else
            {
                _logger.LogError("Wallet ({number}) cannot be processed", model.WalletNumber);
                _channel.BasicNack(basicDeliverEventArgs.DeliveryTag, false, false);
            }
                
        }
    }
}