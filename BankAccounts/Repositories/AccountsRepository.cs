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
                using (var writer = new StreamWriter(TABLE_NAME))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
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
                return null;
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Account>().ToList();
                return records.FirstOrDefault(account => account.Id == accountId);
            }
        }

        public static List<Account> GetAllAccountsFromData()
        {
            if (!File.Exists(TABLE_NAME))
            {
                return null;
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Account>().ToList();
            }
        }
}
}


 