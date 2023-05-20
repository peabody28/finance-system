using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
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

        public void Dispose()
        {
            Container?.StopAsync().Wait();
            Container?.DisposeAsync();
        }
    }
}
