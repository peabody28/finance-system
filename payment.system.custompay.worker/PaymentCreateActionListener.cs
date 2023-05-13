using Newtonsoft.Json;
using payment.system.custompay.worker.Models.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace payment.system.custompay.worker
{
    public class PaymentCreateActionListener : BackgroundService
    {
        private readonly ILogger<PaymentCreateActionListener> logger;

        private readonly IServiceProvider serviceProvider;

        private readonly string _paymentCreateQueueName;

        public PaymentCreateActionListener(ILogger<PaymentCreateActionListener> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;

            _paymentCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:PaymentCreate");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var connectionFactory = serviceProvider.GetRequiredService<ConnectionFactory>();
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ch, basicDeliverEventArgs) =>
            {
                var content = Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray());

                var model = JsonConvert.DeserializeObject<PaymentCreateMessageModel>(content);

                // some actions like external API call to payment system
                logger.LogInformation("Payment with id {id} is processed", model.Id);

                channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(_paymentCreateQueueName, false, consumer);

            await Task.Delay(3000, stoppingToken); 
        }
    }
}