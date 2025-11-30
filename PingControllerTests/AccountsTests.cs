using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Request;
using BanksAccount.CQRS.Accounts.Commands.Create;
using System.Net;
using System.Text;
using System.Text.Json;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Records;
using BankAccounts.Shared.Models.Requests;
using BanksAccount.CQRS.Users.Commands.Create;
using Microsoft.EntityFrameworkCore;

namespace PingControllerTests
{
    public class AccountsTests : IClassFixture<CustomWebApplicationFactory>
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

        public AccountsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateAccount_Success_ReturnsNewAccountData()
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
        
        [Fact]
        public async Task CreateAccountSeeded_Success_ReturnsNewAccountData()
        {
            // Arrange
            await _factory.SeedAsync(async db =>
            {
                db.Users.Add(new UserEntity()
                {
                    Id = 4,
                    BillingAddress = "test",
                    DateOfBirth = new DateTime(),
                    Email = "test",
                    Gender = 0,
                    PhoneNumber = "test",
                    UserLastName = "test",
                    UserName = "Test",
                    BillingCity = "tes"
                });
            });
            
            var request = new AccountRequest
            {
                AccountName = "test name",
                AccountType = AccountType.Checking,
                UserId = 4
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

            var accountEntity = await _factory.GetListFromDbAsync(db =>
                db.Accounts.Where(a => a.UserId == 1).ToListAsync());
                
            Assert.NotNull(accountEntity);
        }
    }
}