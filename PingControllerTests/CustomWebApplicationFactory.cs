using BankAccounts.AppplicationData.Db;
using BankAccounts.Shared.Cashe;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;
using System;
using System.Globalization;
using System.Threading.Tasks;
using BankAccounts.Shared.Clients.CurrencyConver;
using Moq;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Xunit;


namespace PingControllerTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("test_db")
        .WithUsername("test_user")
        .Build();

    // private readonly RedisContainer  _redis = new RedisBuilder()
    //     .WithImage("redis:7-alpine")
    //     .WithPortBinding(0, 6379) // host → container ✅
    //     .Build();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        // await _redis.StartAsync();

        using var scope = Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
        await ctx.Database.MigrateAsync();
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

            services.RemoveAll<DbContextOptions<PostgresDbContext>>();

            // ✅ Получить тестовый порт контейнера Postgres
            // var mappedPort = _postgres.GetMappedPublicPort(5432);

            // ✅ Собрать новый connection string с тестовым портом
            var connectionString = _postgres.GetConnectionString();

            // ✅ Зарегистрировать DbContext с правильным connection string
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
            
            //
            // // Create Redis multiplexer AFTER container port is mapped
            // var port = _redis.GetMappedPublicPort(6379);
            // var endpoint = $"localhost:{port}";
            // var mux = ConnectionMultiplexer.Connect(new ConfigurationOptions
            // {
            //     EndPoints = { endpoint },
            //     AbortOnConnectFail = false
            // });
            //
            // // Seed value
            // mux.GetDatabase().StringSet(
            //     "Cad_rate", 1.31m.ToString(CultureInfo.InvariantCulture));
            //
            // // Register in DI
            // services.AddSingleton<IConnectionMultiplexer>(mux);
        });
    }

    public  async Task DisposeAsync()
    {
        // await _redis.StopAsync();

        await _postgres.DisposeAsync();
        // await _redis.DisposeAsync();
    }
}
