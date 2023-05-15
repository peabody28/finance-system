using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Testing.Integration.Helper.Sqlite;
using wallet.Interfaces.Operations;
using wallet.Repositories;
using wallet.tests.Integration.Core.Constants;

namespace wallet.tests.Integration.Core
{
    public class WalletWebApplicationFactory : SqliteContextWebApplicationFactory<Program, WalletDbContext>
    {
        public WalletWebApplicationFactory()
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }

        public WalletDbContext GetDbContext()
        {
            return Services.CreateScope().ServiceProvider.GetRequiredService<WalletDbContext>();
        }

        private void RemoveRabbitMqOperationService(IServiceCollection services)
        {
            ServiceDescriptor item = services.SingleOrDefault((ServiceDescriptor d) => d.ServiceType.Equals(typeof(IRabbitMqOperation)));
            services.Remove(item);
        }

        private void AddRabbitMqOperationService(IServiceCollection services)
        {
            var mock = new Mock<IRabbitMqOperation>();
            mock.SetupSequence(a => a.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Pass();

            services.AddSingleton(mock.Object);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(delegate (IServiceCollection services)
            {
                RemoveRabbitMqOperationService(services);
                AddRabbitMqOperationService(services);
            });
        }
    }
}
