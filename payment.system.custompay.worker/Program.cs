using payment.system.custompay.worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<PaymentCreateActionListener>();
    })
    .Build();

await host.RunAsync();
