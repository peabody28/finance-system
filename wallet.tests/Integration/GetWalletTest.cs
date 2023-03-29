using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;

namespace wallet.tests.Integration
{
    public class GetWalletTest
    {
        private WebApplicationFactory<Program> factory;

        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            factory = new WebApplicationFactory<Program>();
            client = factory.CreateClient();
        }

        [TearDown]
        public void Teardown()
        {
            client?.Dispose();
            factory?.Dispose();
        }

        [Test]
        public async Task TestUnauthorizedGetWallets()
        {
            // Act
            var result = await client.GetAsync("/wallet/");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task TestUnauthorizedGetWalletByNumber()
        {
            // Act 
            var result = await client.GetAsync("/wallet/<some_number>");

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        // auth 
        // https://mazeez.dev/posts/auth-in-integration-tests
    }
}
