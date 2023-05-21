using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace wallet.tests.Integration.Core
{
    public class RabbitWalletWebApplicationFactory : WalletWebApplicationFactory
    {
        private readonly ConnectionFactory connectionFactory;

        public RabbitWalletWebApplicationFactory(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        private static void RemoveRabbitMqConnectionFactoryService(IServiceCollection services)
        {
            ServiceDescriptor item = services.SingleOrDefault((ServiceDescriptor d) => d.ServiceType.Equals(typeof(ConnectionFactory)));
            services.Remove(item);
        }

        private void AddRabbitMqConnectionFactoryService(IServiceCollection services)
        {
            services.AddSingleton(connectionFactory);
        }

        protected override void PrepareRabbitMq(IServiceCollection services)
        {
            RemoveRabbitMqConnectionFactoryService(services);
            AddRabbitMqConnectionFactoryService(services);
        }
    }
}
