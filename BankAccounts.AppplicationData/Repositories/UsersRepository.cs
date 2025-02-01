using BankAccounts.Exceptions;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using BankAccounts.AppplicationData.Records;

namespace BankAccounts.AppplicationData.Repositories
{
   public class UsersRepository
    {
        private const string TABLE_NAME = "users.csv";

        public void AddUserRecord(List<User> users)
        {
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
                var records = csv.GetRecords<User>().ToList();

                if (records.Any())
                {
                    var record = records.FirstOrDefault(x => x.Id == userId);
                    if (record != null)
                    {
                        return record;
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


        public List<User> GetAllUsersFromData()
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("User table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var results = csv.GetRecords<User>().ToList();

                if (results.Any())
                {
                    return results;
                }
                else
                {
                    throw new NotFoundException("No user records found");
                }
            }
        }

        public User UpdateUserRecord(User user)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("User table do not exist");
            }

            var results = new List<User>();

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                results = csvReader.GetRecords<User>().ToList();
            }

            if (results.Any())
            {
                var record = results.FirstOrDefault(x => x.Id == user.Id);

                if (record != null)
                {
                    record.PhoneNumber = user.PhoneNumber;
                    record.Email = user.Email;
                    record.UserName = user.UserName;
                    record.UserLastName = user.UserLastName;
                    record.DateOfBirth = user.DateOfBirth;
                    record.Gender = user.Gender;
                    record.BillingAddress = user.BillingAddress;


                    var index = results.FindIndex(x => x.Id == user.Id);

                    results[index] = record;

                    using var writer = new StreamWriter(TABLE_NAME);
                    using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    {
                        csvWriter.WriteRecords(results);
                        return record;
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

            var records = new List<User>();

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<User>().ToList();
            }

            if (records.Any())
            {
                var index = records.FindIndex(x => x.Id == userId);
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


        public User GetUserByName(string name)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("User table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<User>().ToList();

                if (records.Any())
                {
                    var record = records.FirstOrDefault(x => x.UserName == name);
                    if (record != null)
                    {
                        return record;
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
    }
}

