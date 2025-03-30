using BankAccounts.Exceptions;
using System.Globalization;
using BankAccounts.AppplicationData.Records;
using System.Runtime.ConstrainedExecution;
using static BankAccounts.Shared.Models.GenderType;
using BankAccounts.Shared.Models;
using BankAccounts.Repositories;
using BankAccounts.AppplicationData.DbContext;
using BankAccounts.Records;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using MongoDB.Driver;

namespace BankAccounts.AppplicationData.Repositories
{
    public interface IUserRepository
    {
        User AddUserRecord(User users);
        Task <User> GetOneUserFromData(string userId);
        Task <List<User>> GetAllUsersFromData();
        Task <User> UpdateUserRecord(UpdateUser user);
        Task DeleteUserFromData(string userId);
       
        
    }

    public class UsersRepository : IUserRepository
    {
        //private const string USER_ID_TRACKER = "userId.txt";

        //private readonly IAccountRepository _accountRepository;
        private readonly MongoDbContext _mongoContext;

        public UsersRepository(
            //IAccountRepository accountRepository,
            MongoDbContext context)
        {
            //   _accountRepository = accountRepository;
            _mongoContext = context;
        }

        public User AddUserRecord(User user)
        {
            //var nextId = GetNextUserID();

            var userEntity = new UserEntity
            {
               // UserId = nextId,
                UserName = user.UserName,
                Email = user.Email,
                UserLastName = user.UserLastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                BillingAddress = user.BillingAddress,
            };

            _mongoContext.Users.InsertOne(userEntity);

            user.UserId = userEntity.UserId;
            return user;
        }


           public async Task <User> GetOneUserFromData(string userId)
          {
             
            var taskResult = await _mongoContext.Users.FindAsync(x => x.UserId == userId);
            var userEntity = taskResult.FirstOrDefault();

            if (userEntity != null)
            {
                var user = new User()
                {
                    UserName = userEntity.UserName,
                    Email = userEntity.Email,
                    UserLastName = userEntity.UserLastName,
                    PhoneNumber = userEntity.PhoneNumber,
                    DateOfBirth = userEntity.DateOfBirth,
                    BillingAddress = userEntity.BillingAddress,

                };

                return user;
            }

            throw new NotFoundException("No users records found");
        }


        public async Task<List<User>> GetAllUsersFromData()
        {

            var documents = await _mongoContext.Users.Find(new BsonDocument()).ToListAsync();

            var userList = new List<User>();

            foreach (var record in documents)
            {
                var user = new User()
                {
                    UserId = record.UserId,
                    UserName = record.UserName,
                    Email = record.Email,
                    UserLastName = record.UserLastName,
                    PhoneNumber = record.PhoneNumber,
                    DateOfBirth = record.DateOfBirth,
                    BillingAddress = record.BillingAddress,
                    Gender = record.Gender

                };

                userList.Add(user);

            }
            return userList;
        }
    

        public async Task <User> UpdateUserRecord(UpdateUser user)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.UserId, user.UserId);

            var update = Builders<UserEntity>.Update
                .Set(x => x.UserName, user.UserName)
                .Set(x => x.Email, user.Email)
                .Set(x => x.UserLastName, user.UserLastName)
                .Set(x => x.PhoneNumber, user.PhoneNumber)
                .Set(x => x.DateOfBirth, user.DateOfBirth)
                .Set(x => x.BillingAddress, user.BillingAddress);

        var updateResult = await _mongoContext.Users.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount == 1)
            {
                var taskResult = await _mongoContext.Users.FindAsync(x => x.UserId == user.UserId);
                var userEntity = taskResult.FirstOrDefault();

                if (userEntity != null)
                {
                    var userUpdate = new User()
                    {
                        UserId = userEntity.UserId,
                        UserName = userEntity.UserName,
                        Email = userEntity.Email,
                        UserLastName = userEntity.UserLastName,
                        PhoneNumber = userEntity.PhoneNumber,
                        DateOfBirth = userEntity.DateOfBirth,
                        BillingAddress = userEntity.BillingAddress,
                        Gender = userEntity.Gender
                    };

                        return userUpdate;
                    }
                }
                 if (updateResult.ModifiedCount > 1)
                 {
                   Console.WriteLine("MEssage to developer(problem with user update)");
                   throw new Exception("Developer exception");
                 }
       
                throw new NotFoundException("No user records found");
            
        }

        public async Task DeleteUserFromData(string userId)
        {
            var deleteResult = await _mongoContext.Users.DeleteOneAsync(x => x.UserId == userId);

            if (deleteResult.DeletedCount == 1)
            {
                return;
            }

            if (deleteResult.DeletedCount > 1)
            {
                Console.WriteLine("MEssage to developer");
                throw new Exception("Developer exception");
            }

            throw new NotFoundException("No users records found");
        }

    }
}



