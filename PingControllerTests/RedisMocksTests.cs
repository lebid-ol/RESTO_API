using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;


namespace PingControllerTests
{
    public class RedisMocksTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IConnectionMultiplexer _mux;

        public RedisMocksTests(CustomWebApplicationFactory factory)
        {
            _mux = factory.Services.GetRequiredService<IConnectionMultiplexer>();
        }

        [Fact]
        public async Task RedisContainer_ShouldReturnSeededCadRate()
        {
            // ✅ Используем то же соединение и ту же фабрику, seed не теряем

            var db = _mux.GetDatabase();

            // Act
            var value = await db.StringGetAsync("Cad_rate");
            var rate = decimal.Parse(value!, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(1.31m, rate);
        }
    }
}
