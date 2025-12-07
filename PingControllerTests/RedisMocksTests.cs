using BankAccounts.Shared.Cashe;
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
        private readonly IRedisCacheClient _redisClient;

        public RedisMocksTests(CustomWebApplicationFactory factory)
        {
            _redisClient = factory.Services.GetRequiredService<IRedisCacheClient>();
        }

        [Fact]
        public async Task RedisMock_ShouldReturnSeededCadRate()
        {
            // Act
            var rate = await _redisClient.GetCadRate();

            // Assert
            Assert.Equal(1.5m, rate); // то значение, которое мы задали в моках
        }
    }
}
