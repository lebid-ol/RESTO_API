using BankAccounts.AppplicationData.Records;
using BankAccounts.Shared.Models;
using BankAccounts.Shared.Models.Requests;
using BanksAccount.CQRS.Users.Commands.Create;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;


    namespace PingControllerTests
    {
        public class UsersTests : IClassFixture<CustomWebApplicationFactory>
        {
            private readonly HttpClient _client;
            private readonly CustomWebApplicationFactory _factory;

            private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
            {
                new JsonStringEnumConverter()
            }
            };

            public UsersTests(CustomWebApplicationFactory factory)
            {
                _factory = factory;
                _client = factory.CreateClient();
            }

            // ---------- POST /api/user ----------

            [Fact]
            public async Task CreateUser_Success_ReturnsNewUser()
            {
                // Arrange
                var request = new UserRequest
                {
                    UserName = "Test",
                    UserLastName = "User",
                    Email = "test.user@example.com",
                    PhoneNumber = "123456789",
                    DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Gender = 0, // Gender enum
                    BillingAddress = "Some address"
                };

                var json = JsonSerializer.Serialize(request, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PostAsync("/api/user", content);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var stringBody = await response.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<UserResponse>(stringBody, _jsonOptions);

                Assert.NotNull(body);
                Assert.True(body.Id > 0);
                Assert.Equal(request.UserName, body.UserName);
                Assert.Equal(request.Email, body.Email);
            }

            // ---------- GET /api/user ----------

            [Fact]
            public async Task GetAllUsers_ReturnsSeededUsers()
            {
                // Arrange – сеем пару пользователей напрямую в БД
                await _factory.SeedAsync(async db =>
                {
                    db.Users.Add(new UserEntity
                    {
                        UserName = "User1",
                        UserLastName = "Test1",
                        Email = "user1@example.com",
                        PhoneNumber = "111111111",
                        DateOfBirth = new DateTime(1991, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        BillingAddress = "Address 1",
                        Gender = 0
                    });

                    db.Users.Add(new UserEntity
                    {
                        UserName = "User2",
                        UserLastName = "Test2",
                        Email = "user2@example.com",
                        PhoneNumber = "222222222",
                        DateOfBirth = new DateTime(1992, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                        BillingAddress = "Address 2",
                        Gender = 0
                    });
                });

                // Act
                var response = await _client.GetAsync("/api/user");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var stringBody = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<UserResponse>>(stringBody, _jsonOptions);

                Assert.NotNull(users);
                Assert.True(users.Count > 0);

                Assert.Contains(users, u => u.Email == "user1@example.com");
                Assert.Contains(users, u => u.Email == "user2@example.com");
            }

        // ---------- GET /api/user/{id} ----------

        [Fact]
        public async Task GetUserById_ReturnsCorrectUser()
        {
            // 1. Сначала создаём пользователя через API
            var createRequest = new UserRequest
            {
                UserName = "Single",
                UserLastName = "User",
                Email = "single.user@example.com",
                PhoneNumber = "333333333",
                DateOfBirth = new DateTime(1993, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                Gender = 0,
                BillingAddress = "Address 3"
            };

            var createJson = JsonSerializer.Serialize(createRequest, _jsonOptions);
            var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");

            var createResponse = await _client.PostAsync("/api/user", createContent);
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            var createdBody = await createResponse.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<UserResponse>(createdBody, _jsonOptions);

            Assert.NotNull(createdUser);
            Assert.True(createdUser!.Id > 0);

            var createdId = createdUser.Id;

            // 2. Вызываем GET /api/user/{id}
            var getResponse = await _client.GetAsync($"/api/user/{createdId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getBody = await getResponse.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserResponse>(getBody, _jsonOptions);

            // 3. Проверяем, что данные совпали
            Assert.NotNull(user);
            Assert.Equal(createdId, user!.Id);
            Assert.Equal(createRequest.UserName, user.UserName);
            Assert.Equal(createRequest.Email, user.Email);
            Assert.Equal(createRequest.BillingAddress, user.BillingAddress);
        }


        // ---------- DELETE /api/user/{id} ----------

        [Fact]
            public async Task DeleteUserById_RemovesUser()
            {
                const string email = "delete.user@example.com";

                // Arrange – сеем юзера, которого будем удалять
                await _factory.SeedAsync(async db =>
                {
                    db.Users.Add(new UserEntity
                    {
                        UserName = "ToDelete",
                        UserLastName = "User",
                        Email = email,
                        PhoneNumber = "444444444",
                        DateOfBirth = new DateTime(1994, 4, 4, 0, 0, 0, DateTimeKind.Utc),
                        BillingAddress = "Address 4",
                        Gender = 0
                    });
                });

                var seededId = await _factory.GetFromDbAsync(async db =>
                    await db.Users
                        .Where(u => u.Email == email)
                        .Select(u => u.Id)
                        .FirstAsync());

                // Act – удаляем
                var deleteResponse = await _client.DeleteAsync($"/api/user/{seededId}");

                // Assert статус
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

                // Проверяем, что в БД его нет
                var userInDb = await _factory.GetFromDbAsync(async db =>
                    await db.Users.FirstOrDefaultAsync(u => u.Id == seededId));

                Assert.Null(userInDb);

                // И что GET /api/user/{id} возвращает 404
                var getResponse = await _client.GetAsync($"/api/user/{seededId}");
                Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
            }
        }
    }

