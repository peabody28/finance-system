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
        private readonly ILogger<WalletCreateActionListener> logger;

        private readonly PaymentApiOperation paymentApiOperation;

        private readonly string walletCreateQueueName;

        private readonly IModel channel;

        public WalletCreateActionListener(ILogger<WalletCreateActionListener> logger, IConfiguration configuration, PaymentApiOperation paymentApiOperation, ConnectionFactory connectionFactory)
        {
            this.logger = logger;
            this.paymentApiOperation = paymentApiOperation;

            walletCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:WalletCreate");

            var connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            SetConsumerHandler(consumer);

            consumer.Model.BasicConsume(walletCreateQueueName, false, consumer);

            return Task.CompletedTask;
        }

        private void SetConsumerHandler(EventingBasicConsumer consumer)
        {
            consumer.Received += async (sender, basicDeliverEventArgs) =>
            {
                var isMessageSuccessfulyProcessed = await TryProcessMessage(basicDeliverEventArgs.Body);

                if (isMessageSuccessfulyProcessed)
                    consumer.Model.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
                else
                    consumer.Model.BasicNack(basicDeliverEventArgs.DeliveryTag, false, false);
            };
        }

        private async Task<bool> TryProcessMessage(ReadOnlyMemory<byte> data)
        {
            var isWalletCreated = false;

            if(TryParseWalletCreateModel(data, out var model))
            {
                isWalletCreated = await paymentApiOperation.TryCreateWallet(model!);

                WalletCreateMessageProcessedLog(model.WalletNumber, isWalletCreated);
            }

            return isWalletCreated;
        }

        private bool TryParseWalletCreateModel(ReadOnlyMemory<byte> data, out WalletCreateDtoModel? model)
        {
            model = default;

            try
            {
                var content = Encoding.UTF8.GetString(data.ToArray());
                model = JsonConvert.DeserializeObject<WalletCreateDtoModel>(content);

                return true;
            }
            catch (Exception) 
            {
                logger.LogError("Model Deserializing Failed, data: {data} ", data);
                return false;
            }
        }

        private void WalletCreateMessageProcessedLog(string walletNumber, bool isSuccess)
        {
            if (isSuccess)
                logger.LogInformation("Wallet ({number}) create message procceed", walletNumber);
            else
                logger.LogError("Wallet ({number}) cannot be processed", walletNumber);
        }
    }
}