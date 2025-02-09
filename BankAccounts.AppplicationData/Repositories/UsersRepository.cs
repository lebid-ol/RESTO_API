using BankAccounts.Exceptions;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using BankAccounts.AppplicationData.Records;
using System.Runtime.ConstrainedExecution;
using static BankAccounts.Shared.Models.GenderType;
using BankAccounts.Shared.Models;
using BankAccounts.Repositories;

namespace BankAccounts.AppplicationData.Repositories
{
    public interface IUserRepository
    {
        User AddUserRecord(User users);
        User GetOneUserFromData(int userId);
        IEnumerable<User> GetAllUsersFromData();
        User UpdateUserRecord(UpdateUser user);
        void DeleteUserFromData(int userId);
    }

    public class UsersRepository : IUserRepository
    {
        private const string TABLE_NAME = "users.csv";
        private const string USER_ID_TRACKER = "userId.txt";

        private readonly IAccountRepository _accountRepository;

        public UsersRepository(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public User AddUserRecord(User user)
        {
            var nextId = GetNextUserID();

            var userEntity = new UserEntity
            {
                UserId = nextId,
                UserName = user.UserName,
                Email = user.Email,
                UserLastName = user.UserLastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                BillingAddress = user.BillingAddress,

            };

            var users = new List<UserEntity>() { userEntity };

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
            }
            user.UserId = nextId;
            return user;


        }

        public User GetOneUserFromData(int userId)
        {
            if (!File.Exists(TABLE_NAME))
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

                        var account = _accountRepository.GetByOwnerId(userId);

                        user.Accounts = new List<Account>() { account };    
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
            }
        }


        public IEnumerable<User> GetAllUsersFromData()
        {
            if (!File.Exists(TABLE_NAME))
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

                    foreach (var record in results)
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
                else
                {
                    throw new NotFoundException("No user records found");
                }
            }
        }

        public User UpdateUserRecord(UpdateUser user)
        {
            if (!File.Exists(TABLE_NAME))
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
            }
        }

        public void DeleteUserFromData(int userId)
        {
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
        }


        private int GetNextUserID()
        {
            if (!File.Exists(USER_ID_TRACKER))
            {
                File.WriteAllText(USER_ID_TRACKER, "1");
                return 1;
            }

            else
            {
                var id = File.ReadAllText(USER_ID_TRACKER);
                var intId = int.Parse(id);
                var nextId = intId + 1;
                File.WriteAllText(USER_ID_TRACKER, nextId.ToString());
                return nextId;
            }
        }
    }
}


