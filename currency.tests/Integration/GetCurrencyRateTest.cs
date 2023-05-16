using currency.Entities;
using currency.Repositories;
using currency.tests.Integration.Core;
using currency.tests.Integration.Core.Constants;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;

namespace currency.tests.Integration
{
    public class GetCurrencyRateTest
    {
        private readonly CurrencyWebApplicationFactory factory = new CurrencyWebApplicationFactory();

        [SetUp]
        public void SetUp()
        {
            factory.SetupDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            factory.DeleteDatabase();
        }

        [Test]
        public async Task GetRateTest()
        {
            // Arrange
            AddCurrencyRateFromUsdToEur();

            var client = factory.CreateClient();

            // Act 
            var result = await client.GetAsync(RouteConstants.FromUsdToEurRate);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private void AddCurrencyRateFromUsdToEur()
        {
            var dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<CurrencyDbContext>();

            var currencyFrom = new CurrencyEntity { Id = Guid.NewGuid(), Code = CurrencyConstants.UsdCurrencyCode };
            var currencyTo = new CurrencyEntity { Id = Guid.NewGuid(), Code = CurrencyConstants.EurCurrencyCode };
            var currencyRate = new CurrencyRateEntity { Id = Guid.NewGuid(), CurrencyFrom = currencyFrom, CurrencyTo = currencyTo, Value = CurrencyConstants.FromUsdToEurRate };
            dbContext.CurrencyRate.Add(currencyRate);

            dbContext.SaveChanges();
        }
    }
}
