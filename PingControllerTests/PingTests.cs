using BankAccounts;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Request;
using BanksAccount.CQRS.Accounts.Commands.Create;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BankAccounts.Shared.Models.Requests;
using BanksAccount.CQRS.Users.Commands.Create;
using Xunit;

namespace PingControllerTests
{
    public class PingTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new System.Text.Json.Serialization.JsonStringEnumConverter()
            }
        };

        public PingTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
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