using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using RabbitMQ.Client;
using wallet.tests.Constants;

namespace wallet.tests.Integration.Core
{
    public class RabbitMqContainerFixture : IDisposable
    {
        private IContainer Container { get; }

        public string HostName { get; }

        public int Port { get; }

        public RabbitMqContainerFixture()
        {
            Container = new ContainerBuilder()
              .WithImage(RabbitMqConstants.RabbitMqDockerImageName)
              .WithPortBinding(RabbitMqConstants.RabbitMqPort, true)
              .Build();

            Container.StartAsync().Wait();

            HostName = Container.Hostname;
            Port = Container.GetMappedPublicPort(RabbitMqConstants.RabbitMqPort);

            Thread.Sleep(RabbitMqConstants.RabbitMqStartDelayMs);
        }

        public ConnectionFactory ConnectionFactory => new ConnectionFactory
        {
            HostName = HostName,
            Port = Port
        };

        public void CreateQueue(string queueName)
        {
            using var connection = ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Dispose()
        {
            Container?.StopAsync().Wait();
            Container?.DisposeAsync();
        }
    }
}
