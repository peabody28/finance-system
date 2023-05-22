using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using payment.Entities;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.Interfaces.Validations;
using payment.Operations;
using payment.Repositories;
using payment.Validations;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Text;
using RabbitMQ.Client;
using payment.Models;
using Microsoft.OpenApi.Models;
using payment.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// RabbitMQ
var factory = new ConnectionFactory()
{
    HostName = builder.Configuration.GetValue<string>("RabbitMq:Host:Name"),
    Port = builder.Configuration.GetValue<int>("RabbitMq:Host:Port"),
    UserName = builder.Configuration.GetValue<string>("RabbitMq:UserName"),
    Password = builder.Configuration.GetValue<string>("RabbitMq:Password"),
    VirtualHost = builder.Configuration.GetValue<string>("RabbitMq:VirtualHostName")
};

builder.Services.AddSingleton(factory);

builder.Services.AddTransient<RabbitMqConnection>();

builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticConfiguration:Uri"]))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"microservices-payment-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        })
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<PaymentDbContext>();
builder.Services.AddScoped<IBalanceOperationTypeRepository, BalanceOperationTypeRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();

builder.Services.AddTransient<IBalanceOperationType, BalanceOperationTypeEntity>();
builder.Services.AddTransient<IPayment, PaymentEntity>();
builder.Services.AddTransient<IWallet, WalletEntity>();
builder.Services.AddTransient<payment.Interfaces.Entities.IConfiguration, ConfigurationEntity>();
builder.Services.AddTransient<ICurrency, CurrencyEntity>();
builder.Services.AddTransient<IPaymentType, PaymentTypeEntity>();

builder.Services.AddScoped<ICurrencyApiOperation, CurrencyApiOperation>();

builder.Services.AddScoped<IConfigurationOperation, ConfigurationOperation>();
builder.Services.AddScoped<IBalanceOperationTypeOperation, BalanceOperationTypeOperation>();
builder.Services.AddScoped<IPaymentOperation, PaymentOperation>();
builder.Services.AddScoped<ICurrencyRateOperation, CurrencyRateOperation>();
builder.Services.AddScoped<IBalanceOperation, BalanceOperation>();
builder.Services.AddScoped<IPaymentTypeOperation, PaymentTypeOperation>();

builder.Services.AddScoped<IRabbitMqOperation, RabbitMqOperation>();

builder.Services.AddScoped<IWalletValidation, WalletValidation>();
builder.Services.AddScoped<IBalanceValidation, BalanceValidation>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("AuthOptions:ISSUER").Value,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("AuthOptions:AUDIENCE").Value,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AuthOptions:KEY").Value)),
        ValidateIssuerSigningKey = true,
    };
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.StopOnFirstFailure;

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });
});

builder.Services.AddHostedService<WalletCreateActionListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API V1");
});

app.Run();
