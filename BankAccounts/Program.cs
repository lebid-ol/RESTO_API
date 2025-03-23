using System.Text.Json.Serialization;
using BankAccounts.AppplicationData.DbContext;
using BankAccounts.AppplicationData.Repositories;
using BankAccounts.Repositories;
using BankAccounts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var confiGbuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var result = confiGbuilder.Build();

builder.Services.AddScoped<IAccountRepository, AccountsRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<MongoDbContext>();

//Add automapper
//builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    options.JsonSerializerOptions.DefaultIgnoreCondition =
        JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();

app.UseSwaggerUI();
app.MapScalarApiReference();
app.UseReDoc(options =>
{
    options.RoutePrefix = "redoc"; // Route to access ReDoc (e.g., /redoc)
    options.SpecUrl("/swagger/v1/swagger.json"); // OpenAPI spec location
    options.DocumentTitle = "My API Documentation";
});
app.Run();
