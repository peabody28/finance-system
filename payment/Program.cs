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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<PaymentDbContext>();
builder.Services.AddScoped<IBalanceOperationTypeRepository, BalanceOperationTypeRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();


builder.Services.AddTransient<IBalanceOperationType, BalanceOperationTypeEntity>();
builder.Services.AddTransient<IPayment, PaymentEntity>();
builder.Services.AddTransient<IWallet, WalletEntity>();
builder.Services.AddTransient<payment.Interfaces.Entities.IConfiguration, ConfigurationEntity>();

builder.Services.AddScoped<IWalletApiOperation, WalletApiOperation>();
builder.Services.AddScoped<IConfigurationOperation, ConfigurationOperation>();
builder.Services.AddScoped<IBalanceOperationTypeOperation, BalanceOperationTypeOperation>();
builder.Services.AddScoped<IPaymentOperation, PaymentOperation>();

builder.Services.AddScoped<IWalletValidation, WalletValidation>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
