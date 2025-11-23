using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using BankAccounts;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace PingControllerTests
{
    public class UnitTestPing : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UnitTestPing(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Ping_Returns_Version_And_Time()
        {
            // Act
            var response = await _client.GetAsync("/api/ping");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadFromJsonAsync<PingResponse>();

            Assert.NotNull(body);
            Assert.Equal("1.0.0", body.Version);
            Assert.NotNull(body.ServerTimeUtc);
        }

        private class PingResponse
        {
            public string Version { get; set; }
            public string ServerTimeUtc { get; set; }
        }
    }
}
