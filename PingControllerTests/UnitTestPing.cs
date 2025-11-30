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
    public class UnitTestPing : IClassFixture<CustomWebApplicationFactory>
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

        public UnitTestPing(CustomWebApplicationFactory factory)
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

        [Fact]
        public async Task CreateAccount_Success_ReturnsNewAccoutnData()
        {
            // Arrange
            var userRequest = new UserRequest()
            {
                BillingAddress = "test",
                DateOfBirth = new DateTime(),
                Email = "test",
                Gender = 0,
                PhoneNumber = "test",
                UserLastName = "test",
                UserName = "Test"
            };
            
            var userJson = JsonSerializer.Serialize(userRequest);
            var userContent = new StringContent(userJson, Encoding.UTF8, "application/json"); // ✅
            var newUserResponse = await _client.PostAsync("/api/user", userContent);
            var userResponseString = await newUserResponse.Content.ReadAsStringAsync();
            
            var userResponse = JsonSerializer.Deserialize<UserResponse>(userResponseString, _jsonOptions);
            
            var request = new AccountRequest
            {
                AccountName = "test name",
                AccountType = AccountType.Checking,
                UserId = userResponse.Id
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // ✅
            
            // Act
            var response = await _client.PostAsync("/api/accounts", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stringBody = await response.Content.ReadAsStringAsync();
            
            var body = JsonSerializer.Deserialize<AccountResponse>(stringBody, _jsonOptions);

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