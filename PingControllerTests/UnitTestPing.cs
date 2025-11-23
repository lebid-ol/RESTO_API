using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BankAccounts;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Request;
using BanksAccount.CQRS.Accounts.Commands.Create;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace PingControllerTests
{
    public class UnitTestPing : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UnitTestPing(CustomWebApplicationFactory factory)
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
        
        [Fact]
        public async Task CreateAccount_Success_ReturnsNewAccoutnData()
        {
            // Act
           var request = new AccountRequest
           {
              AccountName = "test name",
              AccountType = AccountType.Checking,
              UserId = 1
           };
           
           var stringJson = JsonSerializer.Serialize(request);
           var stringContent = new StringContent(stringJson);


           var response = await _client.PostAsync("/api/accounts", stringContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadFromJsonAsync<AccountResponse>();

            Assert.NotNull(body);
            Assert.Equal(body.AccountType, AccountType.Checking);
            Assert.Equal(body.Balance, 100);
        }

        private class PingResponse
        {
            public string Version { get; set; }
            public string ServerTimeUtc { get; set; }
        }
    }
}
