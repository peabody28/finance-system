using payment.worker;
using payment.worker.Operations;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<PaymentApiOperation>();

        var factory = new ConnectionFactory()
        {
            HostName = context.Configuration.GetValue<string>("RabbitMq:Host:Name"),
            Port = context.Configuration.GetValue<int>("RabbitMq:Host:Port"),
            UserName = context.Configuration.GetValue<string>("RabbitMq:UserName"),
            Password = context.Configuration.GetValue<string>("RabbitMq:Password")
        };

        services.AddSingleton(factory);

        services.AddHostedService<WalletCreateActionListener>();
    })
    .Build();

await host.RunAsync();
