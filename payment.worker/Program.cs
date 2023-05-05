using payment.worker;
using payment.worker.Operations;
using RabbitMQ.Client;
using Serilog;
using Serilog.Sinks.Elasticsearch;

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
    .UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"microservices-payment.worker-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            })
            .ReadFrom.Configuration(context.Configuration);
    })
    .Build();

await host.RunAsync();
