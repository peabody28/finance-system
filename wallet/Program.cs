﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Text;
using wallet.Entities;
using wallet.Interfaces.Entities;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;
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
    Password = builder.Configuration.GetValue<string>("RabbitMq:Password")
};

builder.Services.AddSingleton(factory);

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

public partial class Program { }