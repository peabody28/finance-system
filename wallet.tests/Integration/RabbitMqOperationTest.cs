using Newtonsoft.Json;
using NUnit.Framework;
using RabbitMQ.Client;
using System.Text;
using wallet.Models.DTO.RabbitMq;
using wallet.Operations;
using wallet.tests.Constants;
using wallet.tests.Integration.Core;

namespace wallet.tests.Integration
{
    internal class RabbitMqOperationTest
    {
        private RabbitMqContainerFixture containerFixture;

        private ConnectionFactory connectionFactory;

        [SetUp]
        public void Setup()
        {
            StartRabbitMqContainer();
            connectionFactory = GetRabbitMqConnectionFactory();
            CreateQueue(RabbitMqConstants.RabbitMqTestQueueName);
        }

        private void StartRabbitMqContainer()
        {
            containerFixture = new RabbitMqContainerFixture();
        }

        private ConnectionFactory GetRabbitMqConnectionFactory()
        {
            return new ConnectionFactory
            {
                HostName = containerFixture.HostName,
                Port = containerFixture.Port
            };
        }

        private void CreateQueue(string name)
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(name, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        [TearDown]
        public void TearDown() 
        {
            containerFixture.Dispose();
        }

        [Test]
        public void CreateWalletTest()
        {
            // Arrange
            var rabbitMqOperation = new RabbitMqOperation(connectionFactory);

            var messageForSend = new WalletDtoModel { WalletNumber = TestWalletConstants.AnyWalletNumber, CurrencyCode = TestCurrencyConstants.AnyCurrencyCode };

            // Act 
            rabbitMqOperation.SendMessage(messageForSend, RabbitMqConstants.RabbitMqTestQueueName);
            var recievedMessage = GetMessageFromQueue();

            // Assert
            AssertThatMessagesEqual(recievedMessage, messageForSend);
        }

        private WalletDtoModel? GetMessageFromQueue()
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var result = channel.BasicGet(RabbitMqConstants.RabbitMqTestQueueName, true);
            var content = Encoding.UTF8.GetString(result.Body.ToArray());

            return JsonConvert.DeserializeObject<WalletDtoModel?>(content);
        }

        private static void AssertThatMessagesEqual(WalletDtoModel recievedMessage, WalletDtoModel sendedMessage)
        {
            Assert.Multiple(() =>
            {
                Assert.That(recievedMessage.WalletNumber, Is.EqualTo(sendedMessage.WalletNumber));
                Assert.That(recievedMessage.CurrencyCode, Is.EqualTo(sendedMessage.CurrencyCode));
            });
        }
    }
}
