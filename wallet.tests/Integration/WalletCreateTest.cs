using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using System.Text;
using System.Text.Json;
using wallet.Repositories;
using wallet.tests.Integration.Core;
using wallet.tests.Integration.Core.Constants;

namespace wallet.tests.Integration
{
    public class WalletCreateTest
    {
        private readonly WalletWebApplicationFactory factory = new WalletWebApplicationFactory();

        [SetUp]
        public void Setup()
        {
            factory.SetupDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            factory.DeleteDatabase();
        }

        [Test]
        public async Task CreateWalletTest()
        {
            // Arrange
            AddCurrencyToDatabase(TestCurrencyConstants.UsdCurrencyCode);
            var requestContent = GetCreateWalletRequestContent(TestCurrencyConstants.UsdCurrencyCode);

            var client = factory.CreateClient();

            // Act 
            var result = await client.PostAsync("/wallet/", requestContent);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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

        private void AddCurrencyToDatabase(string currencyCode)
        {
            var dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<WalletDbContext>();

            dbContext.Currency.Add(new Entities.CurrencyEntity { Id = Guid.NewGuid(), Code = currencyCode });

            dbContext.SaveChanges();
        }
    }
}
