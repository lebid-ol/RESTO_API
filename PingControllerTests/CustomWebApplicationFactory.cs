using BankAccounts.AppplicationData.Db;
using BankAccounts.Shared.Cashe;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BankAccounts.Shared.Clients.CurrencyConver;
using Moq;
using Testcontainers.PostgreSql;


namespace PingControllerTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("test_db")
        .WithUsername("test_user")
        .Build();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        using var scope = Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
        await ctx.Database.MigrateAsync();
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

            services.RemoveAll<DbContextOptions<PostgresDbContext>>();
            var connectionString = _postgres.GetConnectionString();

            services.AddDbContext<PostgresDbContext>(opt =>
                opt.UseNpgsql(connectionString));

            // Мокаем ICurrencyConverterClient
            services.RemoveAll<ICurrencyConverterClient>();
            var currencyConverterClientMoq = new Mock<ICurrencyConverterClient>();

            currencyConverterClientMoq.Setup(x => x.GetCADRates())
                .ReturnsAsync(1.5m);
            
            services.AddSingleton(currencyConverterClientMoq.Object);
        
            services.RemoveAll<IRedisCacheClient>();
            var redisCacheClienttMoq = new Mock<IRedisCacheClient>();

            redisCacheClienttMoq.Setup(x => x.GetCadRate())
                .ReturnsAsync(1.5m);
            
            services.AddSingleton(currencyConverterClientMoq.Object);
            services.AddSingleton(redisCacheClienttMoq.Object);
        });
    }
    
    public async Task SeedAsync(Func<PostgresDbContext, Task> seedAction)
    {
        using var scope = Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();

        await seedAction(ctx);

        await ctx.SaveChangesAsync();
    }
    
    public async Task<T> GetFromDbAsync<T>(Func<PostgresDbContext, Task<T>> query)
    {
        using var scope = Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();

        return await query(ctx);
    }
    
    public async Task<List<T>> GetListFromDbAsync<T>(Func<PostgresDbContext, Task<List<T>>> query)
    {
        using var scope = Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();

        return await query(ctx);
    }

    public  async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }
}
