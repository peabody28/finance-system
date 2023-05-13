using payment.system.custompay.worker;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var factory = new ConnectionFactory()
        {
            HostName = context.Configuration.GetValue<string>("RabbitMq:Host:Name"),
            Port = context.Configuration.GetValue<int>("RabbitMq:Host:Port"),
            UserName = context.Configuration.GetValue<string>("RabbitMq:UserName"),
            Password = context.Configuration.GetValue<string>("RabbitMq:Password"),
            VirtualHost = context.Configuration.GetValue<string>("RabbitMq:VirtualHostName"),
        };

        services.AddSingleton(factory);

        services.AddHostedService<PaymentCreateActionListener>();
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
                IndexFormat = $"microservices-payment.system.custompay.worker-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            })
            .ReadFrom.Configuration(context.Configuration);
    })
    .Build();

await host.RunAsync();
