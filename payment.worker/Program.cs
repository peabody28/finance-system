using payment.worker;
using payment.worker.Operations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<PaymentApiOperation>();

        services.AddHostedService<WalletCreateActionListener>();
    })
    .Build();

await host.RunAsync();
