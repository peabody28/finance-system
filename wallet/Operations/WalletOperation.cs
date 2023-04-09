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

        public WalletOperation(IConfiguration configuration, IWalletRepository walletRepository, IRabbitMqOperation rabbitMqOperation)
        {
            this.configuration = configuration;
            this.walletRepository = walletRepository;
            this.rabbitMqOperation = rabbitMqOperation;
        }

        public string GenerateNumber()
        {
            var pattern = configuration.GetSection("WalletNumberPattern").Value;

            return RegExpHelper.GenerateString(pattern);
        }

        public IWallet Create(string walletNumber, ICurrency currency, IUser user)
        {
            var wallet = walletRepository.Create(walletNumber, currency, user);

            if(wallet != null)
            {
                var walletCreateQueueName = configuration.GetValue<string>("RabbitMq:Queue:WalletCreate");
                var walletDtoModel = new WalletDtoModel { WalletNumber = walletNumber, CurrencyCode = wallet.Currency.Code };
                rabbitMqOperation.SendMessage(walletDtoModel, walletCreateQueueName);
            }

            return wallet;
        }
    }
}
