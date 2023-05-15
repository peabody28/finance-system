using currency.Entities;
using currency.Repositories;
using currency.tests.Integration.Core;
using currency.tests.Integration.Core.Constants;
using NUnit.Framework;
using System.Net;

namespace currency.tests.Integration
{
    public class GetCurrencyRateTest
    {
        private readonly CurrencyWebApplicationFactory factory = new CurrencyWebApplicationFactory();

        [SetUp]
        public void Setup()
        {
            AddCurrencyRateFromUsdToEur();
        }

        [TearDown]
        public void Teardown()
        {
            factory?.Dispose();
        }

        [Test]
        public async Task GetRateTest()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act 
            var result = await client.GetAsync(RouteConstants.FromUsdToEurRate);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private CurrencyDbContext SetupDbContext()
        {
            var dbContext = factory.GetDbContext();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private void AddCurrencyRateFromUsdToEur()
        {
            var dbContext = SetupDbContext();

            var currencyFrom = new CurrencyEntity { Id = Guid.NewGuid(), Code = CurrencyConstants.UsdCurrencyCode };
            var currencyTo = new CurrencyEntity { Id = Guid.NewGuid(), Code = CurrencyConstants.EurCurrencyCode };
            var currencyRate = new CurrencyRateEntity { Id = Guid.NewGuid(), CurrencyFrom = currencyFrom, CurrencyTo = currencyTo, Value = CurrencyConstants.FromUsdToEurRate };
            dbContext.CurrencyRate.Add(currencyRate);

            dbContext.SaveChanges();
        }
    }
}
