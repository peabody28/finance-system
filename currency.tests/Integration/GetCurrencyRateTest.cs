using currency.Entities;
using currency.Repositories;
using currency.tests.Integration.Core;
using Microsoft.Extensions.DependencyInjection;
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

            var context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<CurrencyDbContext>();

            context.Database.EnsureCreated();

            var currencyFrom = new CurrencyEntity { Id = Guid.NewGuid(), Code = "USD" };
            var currencyTo = new CurrencyEntity { Id = Guid.NewGuid(), Code = "EUR" };

            var currencyRate = new CurrencyRateEntity { Id = Guid.NewGuid(), CurrencyFrom = currencyFrom, CurrencyTo = currencyTo, Value = 0.94m };

            context.CurrencyRate.Add(currencyRate);

            context.SaveChanges();
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
    }
}
