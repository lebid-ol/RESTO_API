using BankAccounts.Exceptions;
using BankAccounts.Records;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAccounts.AppplicationData.Records;

namespace BankAccounts.AppplicationData.Repositories
{
   public class UsersRepository
    {
        private const string TABLE_NAME = "users.csv";

        /*public void AddAcountRecord(List<Account> accounts)
        {
            if (!File.Exists(TABLE_NAME))
            {
                using var writer = new StreamWriter(TABLE_NAME);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                {
                    csv.WriteRecords(accounts);
                }
            }
            else
            {
                // Append to the file.
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,

                };

                using (var stream = File.Open(TABLE_NAME, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(accounts);
                }
            }


        }

        public Account GetOneAccountFromData(int accountId)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Account>().ToList();

                if (records.Any())
                {
                    var record = records.FirstOrDefault(x => x.Id == accountId);
                    if (record != null)
                    {
                        return record;
                    }
                    else
                    {
                        throw new NotFoundException("No account records found");
                    }
                }
                else
                {
                    throw new NotFoundException("No account records found");
                }
            }
        }
        */

        public List<User> GetAllUsersFromData()
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
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
                    throw new NotFoundException("No account records found");
                }
            }
        }

       /* public Account UpdateAccountRecord(Account account)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            var results = new List<Account>();

            // получить все данные
            using (var reader = new StreamReader(TABLE_NAME))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                results = csvReader.GetRecords<Account>().ToList();
            }

            if (results.Any())
            {
                var record = results.FirstOrDefault(x => x.Id == account.Id);

                if (record != null)
                {
                    record.AccountName = account.AccountName;
                    record.AccountEmail = account.AccountEmail;
                    record.UserName = account.UserName;
                    record.UserLastName = account.UserLastName;

                    var index = results.FindIndex(x => x.Id == account.Id);

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
                    throw new NotFoundException("No account records found");
                }
            }
            else
            {
                throw new NotFoundException("No account records found");
            }
        }

        public void DeleteAccountFromData(int accountId)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            var records = new List<Account>();

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Account>().ToList();
            }

            if (records.Any())
            {
                var index = records.FindIndex(x => x.Id == accountId);
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
                    throw new NotFoundException("No account records found");
                }
            }
            else
            {
                throw new NotFoundException("No account records found");
            }
        }
       */

        public Account GetAccountByName(string name)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Account>().ToList();

                if (records.Any())
                {
                    var record = records.FirstOrDefault(x => x.AccountName == name);
                    if (record != null)
                    {
                        return record;
                    }
                    else
                    {
                        throw new NotFoundException("No account records found");
                    }
                }
                else
                {
                    throw new NotFoundException("No account records found");
                }
            }
        }
    }
}
}
