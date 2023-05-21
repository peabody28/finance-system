using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;
using wallet.Models.DTO.RabbitMq;
using wallet.Operations;
using wallet.tests.Constants;
using wallet.tests.Integration.Core;

namespace wallet.tests.Unit
{
    internal class RabbitMqOperationTest
    {
        private RabbitMqContainerFixture containerFixture;

        [SetUp]
        public void Setup()
        {
            containerFixture = new RabbitMqContainerFixture();
            containerFixture.CreateQueue(RabbitMqConstants.RabbitMqTestQueueName);
        }

        [TearDown]
        public void TearDown()
        {
            containerFixture?.Dispose();
        }

        [Test]
        public void SendMessageTest()
        {
            // Arrange
            var rabbitMqOperation = new RabbitMqOperation(containerFixture.ConnectionFactory);

            var messageForSend = new WalletDtoModel { WalletNumber = TestWalletConstants.AnyWalletNumber, CurrencyCode = TestCurrencyConstants.AnyCurrencyCode };

            // Act 
            rabbitMqOperation.SendMessage(messageForSend, RabbitMqConstants.RabbitMqTestQueueName);
            var recievedMessage = GetMessageFromQueue();

            // Assert
            AssertThatMessagesEqual(recievedMessage, messageForSend);
        }

        private WalletDtoModel? GetMessageFromQueue()
        {
            using var connection = containerFixture.ConnectionFactory.CreateConnection();
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
