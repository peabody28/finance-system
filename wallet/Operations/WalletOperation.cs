using wallet.Helpers;
using wallet.Interfaces.Entities;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;
using wallet.Models.DTO.RabbitMq;

namespace wallet.Operations
{
    public class WalletOperation : IWalletOperation
    {
        private readonly IConfiguration configuration;

        private readonly IWalletRepository walletRepository;

        private readonly IRabbitMqOperation rabbitMqOperation;

        private readonly ILogger<WalletOperation> logger;

        public WalletOperation(IConfiguration configuration, IWalletRepository walletRepository, IRabbitMqOperation rabbitMqOperation, ILogger<WalletOperation> logger)
        {
            this.configuration = configuration;
            this.walletRepository = walletRepository;
            this.rabbitMqOperation = rabbitMqOperation;
            this.logger = logger;

        }

        public string GenerateNumber()
        {
            var pattern = configuration.GetValue<string>("WalletNumberPattern");

            return RegExpHelper.GenerateString(pattern);
        }

        public IWallet Create(string walletNumber, ICurrency currency, IUser user)
        {
            var wallet = walletRepository.Create(walletNumber, currency, user);

            SendRabbitMqCreateWalletMessage(wallet);

            return wallet;
        }

        private void SendRabbitMqCreateWalletMessage(IWallet wallet)
        {
            var walletCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:WalletCreate");
            var walletDtoModel = new WalletDtoModel { WalletNumber = wallet.Number, CurrencyCode = wallet.Currency.Code };
            rabbitMqOperation.SendMessage(walletDtoModel, walletCreateQueueName);

            logger.LogInformation("Wallet created: userName: {userName}, walletNumber: {walletNumber}, currencyCode: {currency}",
                wallet.User.Name, wallet.Number, wallet.Currency.Code);
        }
    }
}
