using BankAccounts.Exceptions;
using BankAccounts.Records;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace BankAccounts.Repositories
{
    public static class AccountsRepository
    {
        private const string TABLE_NAME = "accounts.csv";

        public static void AddAcountRecord(List<Account> accounts)
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

        public static Account GetOneAccountFromData(int accountId)
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

        public static List<Account> GetAllAccountsFromData()
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var results = csv.GetRecords<Account>().ToList();
               
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

        public static void UpdateAccountRecord(Account account)
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

    }
}


 