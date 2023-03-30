﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using wallet.Entities;
using wallet.Repositories;
using wallet.tests.Integration.Core;

namespace wallet.tests.Integration
{
    public class GetWalletTest
    {
        private WalletWebApplicationFactory<Program, WalletDbContext> factory;

        private string WalletNumber = "ASDF546F";

        [SetUp]
        public void Setup()
        {
            factory = new WalletWebApplicationFactory<Program, WalletDbContext>();

            var context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<WalletDbContext>();

            context.Database.EnsureCreated();

            var user = new UserEntity { Id = Guid.NewGuid(), Name = factory.UserName };
            var currency = new CurrencyEntity { Id = Guid.NewGuid(), Code = "USD" };
            var wallet = new WalletEntity { Id = Guid.NewGuid(), Currency = currency, Number = WalletNumber, User = user };

            context.Wallet.Add(wallet);

            context.SaveChanges();
        }

        [TearDown]
        public void Teardown()
        {
            factory?.Dispose();
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

        /// <summary>
        /// Only for MS role
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetWalletByNumberTest()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act 
            var result = await client.GetAsync($"/wallet/{WalletNumber}");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        /// <summary>
        /// Only for MS role
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUndefinedWalletByNumberTest()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act 
            var result = await client.GetAsync("/wallet/XXX");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
