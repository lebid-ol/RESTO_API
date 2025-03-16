using BankAccounts.Exceptions;
using CsvHelper.Configuration;
using CsvHelper;
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
        User GetOneUserFromData(int userId);
        Task <List<User>> GetAllUsersFromData();
        User UpdateUserRecord(UpdateUser user);
        void DeleteUserFromData(int userId);
       
        
    }

    public class UsersRepository : IUserRepository
    {
        private const string TABLE_NAME = "users.csv";
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
            var nextId = GetNextUserID();

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

            /*var users = new List<UserEntity>() { userEntity };

            if (!File.Exists(TABLE_NAME))
            {
                using var writer = new StreamWriter(TABLE_NAME);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                {
                    csv.WriteRecords(users);
                }
            }
            else
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };

                using (var stream = File.Open(TABLE_NAME, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(users);
                }
            */
            user.UserId = userEntity.UserId;
            return user;
        }


           public User GetOneUserFromData(int userId)
          {
            throw new NotImplementedException();
            /* if (!File.Exists(TABLE_NAME))
             {
                 throw new DontExistException("User table do not exist");
             }

             using (var reader = new StreamReader(TABLE_NAME))
             using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
             {
                 var records = csv.GetRecords<UserEntity>().ToList();

                 if (records.Any())
                 {
                     var record = records.FirstOrDefault(x => x.UserId == userId);
                     if (record != null)
                     {
                         var user = new User()
                         {
                             UserName = record.UserName,
                             Email = record.Email,
                             UserLastName = record.UserLastName,
                             PhoneNumber = record.PhoneNumber,
                             DateOfBirth = record.DateOfBirth,
                             BillingAddress = record.BillingAddress,

                         };

                         var account = _accountRepository.GetAllAccountsByOwnerID(userId);
                         user.Accounts = account;

                         return user;

                     }
                     else
                     {
                         throw new NotFoundException("No user records found");
                     }
                 }
                 else
                 {
                     throw new NotFoundException("No user records found");
                 }
             }*/
        }


        public async Task<List<User>> GetAllUsersFromData()
        {


            /* if (File.Exists(TABLE_NAME))
            {
                throw new DontExistException("User table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var results = csv.GetRecords<UserEntity>().ToList();

                if (results.Any())
                {
                    var userList = new List<User>();
            */

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

        /*  else
          {
              throw new NotFoundException("No user records found");
          }
      }*/
    

        public User UpdateUserRecord(UpdateUser user)
        {
        throw new NotImplementedException();
       /* if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("User table do not exist");
            }

            var results = new List<UserEntity>();

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                results = csvReader.GetRecords<UserEntity>().ToList();
            }

            if (results.Any())
            {
                var record = results.FirstOrDefault(x => x.UserId == user.UserId);

                if (record != null)
                {
                    record.PhoneNumber = user.PhoneNumber;
                    record.Email = user.Email;
                    record.UserName = user.UserName;
                    record.UserLastName = user.UserLastName;
                    record.DateOfBirth = user.DateOfBirth;
                    record.BillingAddress = user.BillingAddress;


                    var index = results.FindIndex(x => x.UserId == user.UserId);

                    results[index] = record;

                    using var writer = new StreamWriter(TABLE_NAME);
                    using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    {
                        csvWriter.WriteRecords(results);

                        var updateUser = new User()
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

                        return updateUser;
                    }
                }
                else
                {
                    throw new NotFoundException("No user records found");
                }
            }
            else
            {
                throw new NotFoundException("No user records found");
            }*/
        }

        public void DeleteUserFromData(int userId)
        {
            throw new NotImplementedException();
            /*
                if (!File.Exists(TABLE_NAME))
                {
                    throw new DontExistException("User table do not exist");
                }

                var records = new List<UserEntity>();

                using (var reader = new StreamReader(TABLE_NAME))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    records = csv.GetRecords<UserEntity>().ToList();
                }

                if (records.Any())
                {
                    var index = records.FindIndex(x => x.UserId == userId);
                    if (index != -1)
                    {
                        records.RemoveAt(index);

                        using (var writer = new StreamWriter(TABLE_NAME))
                        using (var csvToUpdate = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csvToUpdate.WriteRecords(records);
                        }

                    }
                    else
                    {
                        throw new NotFoundException("No user records found");
                    }
                }
                else
                {
                    throw new NotFoundException("No user records found");
                }
            */
        }


        private int GetNextUserID()
        {
            if (!File.Exists("userId.txt"))
            {
                File.WriteAllText("userId.txt", "1");
                return 1;
            }
            else
            {
                var id = File.ReadAllText("userId.txt");
                var intId = int.Parse(id);
                var nextId = intId + 1;
                File.WriteAllText("userId.txt", nextId.ToString());
                return nextId;
            }
        }
    }
}


