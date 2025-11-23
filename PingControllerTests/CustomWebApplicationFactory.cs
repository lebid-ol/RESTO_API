using BankAccounts.AppplicationData.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace PingControllerTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _postgres.GetConnectionString();
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<PostgresDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<PostgresDbContext>(option =>
            {
                option.UseNpgsql(_postgres.GetConnectionString());
            });

        });
    }

    public Task InitializeAsync()
    {
        return _postgres.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _postgres.DisposeAsync().AsTask();
    }
}