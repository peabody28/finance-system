using NUnit.Framework;
using System.Net;
using System.Text;
using wallet.Repositories;
using wallet.tests.Integration.Core;

namespace wallet.tests.Integration
{
    public class WalletCreateTest
    {
        private WalletWebApplicationFactory factory;

        [SetUp]
        public void Setup()
        {
            factory = new WalletWebApplicationFactory();

            var dbContext = factory.GetDbContext();

            SetupDbContext(dbContext);
        }

        [TearDown]
        public void Teardown()
        {
            factory?.Dispose();
        }

        [Test]
        public async Task CreateWalletTest()
        {
            // Arrange
            var payload = "{\"currencyCode\": \"USD\" }";

            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            var client = factory.CreateClient();

            // Act 
            var result = await client.PostAsync("/wallet/", content);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private static void SetupDbContext(WalletDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            AddCurrencyRowToDatabase(dbContext);
        }

        private static void AddCurrencyRowToDatabase(WalletDbContext dbContext)
        {
            dbContext.Currency.Add(new Entities.CurrencyEntity { Id = Guid.NewGuid(), Code = "USD" });

            dbContext.SaveChanges();
        }
    }
}
