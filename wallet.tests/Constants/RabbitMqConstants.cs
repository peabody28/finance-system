namespace wallet.tests.Constants
{
    internal class RabbitMqConstants
    {
        public const string RabbitMqDockerImageName = "rabbitmq:latest";

        public const int RabbitMqPort = 5672;

        public const int RabbitMqStartDelayMs = 10000;

        public const int RabbitMqReadMessageDelayMs = 1000;

        public const string RabbitMqTestQueueName = "wallet-create";
    }
}
