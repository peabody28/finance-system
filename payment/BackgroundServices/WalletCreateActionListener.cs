using Newtonsoft.Json;
using payment.Interfaces.Repositories;
using payment.Models.DTO.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace payment.BackgroundServices
{
    public class WalletCreateActionListener : BackgroundService
    {
        private readonly ILogger<WalletCreateActionListener> logger;

        private readonly string walletCreateQueueName;

        private readonly IModel channel;

        private readonly IServiceProvider serviceProvider;

        private ICurrencyRepository CurrencyRepository => serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICurrencyRepository>();

        private IWalletRepository WalletRepository => serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IWalletRepository>();

        public WalletCreateActionListener(ILogger<WalletCreateActionListener> logger, IServiceProvider serviceProvider, IConfiguration configuration, ConnectionFactory connectionFactory)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
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
            var isWalletCreated = false;

            if(TryParseWalletCreateModel(data, out var model))
            {
                isWalletCreated = CreateWallet(model.WalletNumber, model.CurrencyCode);

                WalletCreateMessageProcessedLog(model.WalletNumber, isWalletCreated);
            }

            return isWalletCreated;
        }

        private bool CreateWallet(string walletNumber, string currencyCode)
        {
            var currency = CurrencyRepository.GetOrCreate(currencyCode);
            var wallet = WalletRepository.Create(walletNumber, currency);

            return wallet != null;
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