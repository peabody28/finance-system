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

        private readonly IModel channel;

        private readonly string _paymentCreateQueueName;

        public PaymentCreateActionListener(ILogger<PaymentCreateActionListener> logger, IConfiguration configuration, ConnectionFactory connectionFactory)
        {
            this.logger = logger;

            _paymentCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:PaymentCreate");

            var connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            SetConsumerHandler(consumer);

            channel.BasicConsume(_paymentCreateQueueName, false, consumer);

            return Task.CompletedTask;
        }

        private void SetConsumerHandler(EventingBasicConsumer consumer)
        {
            consumer.Received += (sender, basicDeliverEventArgs) =>
            {
                var isMessageSuccessfulyProcessed = TryProcessMessage(basicDeliverEventArgs.Body);

                if (isMessageSuccessfulyProcessed)
                    consumer.Model.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
                else
                    consumer.Model.BasicNack(basicDeliverEventArgs.DeliveryTag, false, false);
            };
        }

        private bool TryProcessMessage(ReadOnlyMemory<byte> data)
        {
            var isPaymentProcessed = false;

            if (TryParsePaymentCreateModel(data, out var model))
            {
                isPaymentProcessed = true;

                PaymentCreateMessageProcessedLog(model.Id, isPaymentProcessed);
            }

            return isPaymentProcessed;
        }

        private bool TryParsePaymentCreateModel(ReadOnlyMemory<byte> data, out PaymentCreateMessageModel? model)
        {
            model = default;

            try
            {
                var content = Encoding.UTF8.GetString(data.ToArray());
                model = JsonConvert.DeserializeObject<PaymentCreateMessageModel>(content);

                return true;
            }
            catch (Exception)
            {
                logger.LogError("Model Deserializing Failed, data: {data} ", data);
                return false;
            }
        }

        private void PaymentCreateMessageProcessedLog(Guid paymentId, bool isSuccess)
        {
            if (isSuccess)
                logger.LogInformation("Payment ({id}) create message procceed", paymentId);
            else
                logger.LogError("Payment ({id}) cannot be processed", paymentId);
        }
    }

}