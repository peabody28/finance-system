using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Text;
using wallet.Entities;
using wallet.Interfaces.Entities;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;
using wallet.Models;
using wallet.Operations;
using wallet.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<WalletDbContext>(optionsBuilder =>
{
    var connString = builder.Configuration.GetConnectionString("Wallet");
    optionsBuilder.UseSqlite(connString);
});

builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

builder.Services.AddTransient<IUser, UserEntity>();
builder.Services.AddTransient<ICurrency, CurrencyEntity>();
builder.Services.AddTransient<IWallet, WalletEntity>();

builder.Services.AddScoped<IWalletOperation, WalletOperation>();
builder.Services.AddScoped<IUserOperation, UserOperation>();
builder.Services.AddSingleton<IRabbitMqOperation, RabbitMqOperation>();

// RabbitMQ
var factory = new ConnectionFactory()
{
    HostName = builder.Configuration.GetValue<string>("RabbitMq:Host:Name"),
    Port = builder.Configuration.GetValue<int>("RabbitMq:Host:Port"),
    UserName = builder.Configuration.GetValue<string>("RabbitMq:UserName"),
    Password = builder.Configuration.GetValue<string>("RabbitMq:Password"),
    VirtualHost = builder.Configuration.GetValue<string>("RabbitMq:VirtualHostName"),
};

builder.Services.AddSingleton(factory);

builder.Services.AddTransient<RabbitMqConnection>();

Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticConfiguration:Uri"]))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"microservices-wallet-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        })
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

builder.Host.UseSerilog();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.StopOnFirstFailure;

builder.Services.AddAuthorization();

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

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet API V1");
});

app.Run();

public partial class Program { }