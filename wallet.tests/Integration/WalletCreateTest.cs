using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using System.Text;
using System.Text.Json;
using wallet.Models.DTO.RabbitMq;
using wallet.Repositories;
using wallet.tests.Constants;
using wallet.tests.Integration.Core;

namespace wallet.tests.Integration
{
    public class WalletCreateTest
    {
        private RabbitMqContainerFixture containerFixture;

        private RabbitWalletWebApplicationFactory factory;

        #region [ Setup/Teardown ]

        [OneTimeSetUp]
        public void PrepareEnvironment()
        {
            containerFixture = new RabbitMqContainerFixture();
            containerFixture.CreateQueue(RabbitMqConstants.RabbitMqTestQueueName);
            factory = new RabbitWalletWebApplicationFactory(containerFixture.ConnectionFactory);
        }

        [OneTimeTearDown]
        public void ResetEnvironment()
        {
            containerFixture?.Dispose();
            factory?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            factory.SetupDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            factory?.DeleteDatabase();
        }

        #endregion

        [Test]
        public async Task CreateWalletTest()
        {
            // Arrange
            AddCurrencyToDatabase(TestCurrencyConstants.UsdCurrencyCode);
            var requestContent = GetCreateWalletRequestContent(TestCurrencyConstants.UsdCurrencyCode);

            var client = factory.CreateClient();

            // Act 
            var result = await client.PostAsync("/wallet/", requestContent);
            var rabbitMessage = GetMessageFromQueue(RabbitMqConstants.RabbitMqTestQueueName);

            // Assert
            AssertThatStatusIsOkAndRabbitStoreMessage(result, rabbitMessage);
        }

        private void AddCurrencyToDatabase(string currencyCode)
        {
            var dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<WalletDbContext>();

            dbContext.Currency.Add(new Entities.CurrencyEntity { Id = Guid.NewGuid(), Code = currencyCode });

            dbContext.SaveChanges();
        }

        private static HttpContent GetCreateWalletRequestContent(string walletCurrencyCode)
        {
            var createWalletRequest = new
            {
                currencyCode = walletCurrencyCode
            };

            string jsonString = JsonSerializer.Serialize(createWalletRequest);

            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        private WalletDtoModel? GetMessageFromQueue(string queueName)
        {
            using var connection = containerFixture.ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var result = channel.BasicGet(queueName, true);
            var content = Encoding.UTF8.GetString(result.Body.ToArray());

            return Newtonsoft.Json.JsonConvert.DeserializeObject<WalletDtoModel?>(content);
        }

        private static void AssertThatStatusIsOkAndRabbitStoreMessage(HttpResponseMessage httpResponse, WalletDtoModel? rabbitMessage)
        {
            Assert.Multiple(() =>
            {
                Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(rabbitMessage, Is.Not.Null);
            });
        }
    }
}
