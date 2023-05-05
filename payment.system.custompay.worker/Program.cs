using payment.system.custompay.worker;
using Serilog.Sinks.Elasticsearch;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
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
