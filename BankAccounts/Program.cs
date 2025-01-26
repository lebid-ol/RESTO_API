using BankAccounts.API.DI_test;
using BankAccounts.Repositories;
using BankAccounts.Services;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<AccountsRepository>();
builder.Services.AddScoped<AccountService>();


builder.Services.AddControllers();
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
app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});
app.UseSwaggerUI();
app.MapScalarApiReference();
app.UseReDoc(options =>
{
    options.RoutePrefix = "redoc"; // Route to access ReDoc (e.g., /redoc)
    options.SpecUrl("/swagger/v1/swagger.json"); // OpenAPI spec location
    options.DocumentTitle = "My API Documentation";
});
app.Run();
