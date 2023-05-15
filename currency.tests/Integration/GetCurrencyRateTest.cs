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
        private CurrencyWebApplicationFactory factory;

        [SetUp]
        public void Setup()
        {
            factory = new CurrencyWebApplicationFactory();

            var context = factory.GetDbContext();

            SetupDatabaseContext(context);
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
            var result = await client.GetAsync("/currency/rate?currencyFromCode=USD&currencyToCode=EUR");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private static void AddCurrencyRateRow(CurrencyDbContext dbContext)
        {
            var currencyFrom = new CurrencyEntity { Id = Guid.NewGuid(), Code = CurrencyConstants.CurrencyFromCode };
            var currencyTo = new CurrencyEntity { Id = Guid.NewGuid(), Code = CurrencyConstants.CurrencyToCode };
            var currencyRate = new CurrencyRateEntity { Id = Guid.NewGuid(), CurrencyFrom = currencyFrom, CurrencyTo = currencyTo, Value = CurrencyConstants.FromUsdToEurRate };
            dbContext.CurrencyRate.Add(currencyRate);

            dbContext.SaveChanges();
        }

        private static void SetupDatabaseContext(CurrencyDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            AddCurrencyRateRow(dbContext);
        }
    }
}
