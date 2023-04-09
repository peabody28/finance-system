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

        private readonly IServiceProvider serviceProvider;

        private readonly PaymentApiOperation paymentApiOperation;

        private readonly string _walletCreateQueueName;

        public WalletCreateActionListener(ILogger<WalletCreateActionListener> logger, IConfiguration configuration, PaymentApiOperation paymentApiOperation, IServiceProvider serviceProvider)
        {
            _logger = logger;
            this.paymentApiOperation = paymentApiOperation;
            this.serviceProvider = serviceProvider;

            _walletCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:WalletCreate");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var connectionFactory = serviceProvider.GetRequiredService<ConnectionFactory>();
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();

            var consumer = new EventingBasicConsumer(model);

            consumer.Received += async (sender, basicDeliverEventArgs) =>
            {
                var content = Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray());

                var model = JsonConvert.DeserializeObject<WalletCreateDtoModel>(content);

                var status = await paymentApiOperation.TryCreateWallet(model);

                if(status)
                {
                    _logger.LogInformation("Wallet ({number}) create message procceed", model.WalletNumber);
                    consumer.Model.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
                }
                else
                {
                    _logger.LogError("Wallet ({number}) cannot be processed", model?.WalletNumber);
                    consumer.Model.BasicNack(basicDeliverEventArgs.DeliveryTag, false, false);
                }
            };

            consumer.Model.BasicConsume(_walletCreateQueueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}