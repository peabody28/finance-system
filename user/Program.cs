using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using user.Entities;
using user.Interfaces.Entities;
using user.Interfaces.Operations;
using user.Interfaces.Repositories;
using user.Operations;
using user.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddTransient<IUser, UserEntity>();
builder.Services.AddTransient<IRole, RoleEntity>();

builder.Services.AddScoped<IIdentityOperation, IdentityOperation>();
builder.Services.AddScoped<IJwtTokenOperation, JwtTokenOperation>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.StopOnFirstFailure;

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
});

app.Run();
