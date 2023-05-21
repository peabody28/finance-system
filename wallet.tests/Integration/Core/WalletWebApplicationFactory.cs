using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Testing.Integration.Helper.Sqlite;
using wallet.Interfaces.Operations;
using wallet.Repositories;
using wallet.tests.Constants;

namespace wallet.tests.Integration.Core
{
    public class WalletWebApplicationFactory : SqliteContextWebApplicationFactory<Program, WalletDbContext>
    {
        public WalletWebApplicationFactory()
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }

        private static void RemoveRabbitMqOperationService(IServiceCollection services)
        {
            ServiceDescriptor item = services.SingleOrDefault((ServiceDescriptor d) => d.ServiceType.Equals(typeof(IRabbitMqOperation)));
            services.Remove(item);
        }

        private static void AddRabbitMqOperationService(IServiceCollection services)
        {
            var mock = new Mock<IRabbitMqOperation>();
            mock.SetupSequence(a => a.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Pass();

            services.AddSingleton(mock.Object);
        }

        protected virtual void PrepareRabbitMq(IServiceCollection services)
        {
            RemoveRabbitMqOperationService(services);
            AddRabbitMqOperationService(services);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(delegate (IServiceCollection services)
            {
                PrepareRabbitMq(services);
            });
        }

        public void SetupDatabase()
        {
            var dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<WalletDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public void DeleteDatabase()
        {
            var dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<WalletDbContext>();
            dbContext.Database.EnsureDeleted();
        }
    }
}
