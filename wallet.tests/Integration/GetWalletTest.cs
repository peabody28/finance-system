using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using wallet.Entities;
using wallet.Repositories;
using wallet.tests.Constants;
using wallet.tests.Integration.Core;

namespace wallet.tests.Integration
{
    public class GetWalletTest
    {
        private readonly WalletWebApplicationFactory factory = new WalletWebApplicationFactory();

        [SetUp]
        public void Setup()
        {
            factory?.SetupDatabase();
        }

        [TearDown]
        public void Teardown()
        {
            factory?.DeleteDatabase();
        }

        [Test]
        public async Task GetWalletsTest()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act 
            var result = await client.GetAsync("/wallet/");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetWalletByNumberTest()
        {
            // Arrange
            AddWalletToDatabase(factory.UserName, TestWalletConstants.WalletNumber);
            var client = factory.CreateClient();

            // Act 
            var result = await client.GetAsync($"/wallet/{TestWalletConstants.WalletNumber}");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetUndefinedWalletByNumberTest()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act 
            var result = await client.GetAsync($"/wallet/{TestWalletConstants.UndefinedWalletNumber}");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private void AddWalletToDatabase(string walletOwnerUserName, string walletNumber)
        {
            var dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<WalletDbContext>();

            var user = new UserEntity { Id = Guid.NewGuid(), Name = walletOwnerUserName };
            var currency = new CurrencyEntity { Id = Guid.NewGuid(), Code = TestCurrencyConstants.AnyCurrencyCode };
            var wallet = new WalletEntity { Id = Guid.NewGuid(), Currency = currency, Number = walletNumber, User = user };
            dbContext.Wallet.Add(wallet);

            dbContext.SaveChanges();
        }
    }
}
