using BankAccounts.AppplicationData.DbContext;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.Shared.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace BankAccounts.Repositories
{
    public interface IAccountRepository
    {
        Account AddAcountRecord(Account accounts);
        Account GetOneAccountFromData(int accountId);
        List<Account> GetAllAccountsFromData();
        Account UpdateAccountRecord(UpdateAccount account);
        void DeleteAccountFromData(int accountId);
        Account GetByOwnerId(int ownerId);
        List <Account> GetAllAccountsByOwnerID(int ownerId);
    }

    public class AccountsRepository : IAccountRepository
    {
        private const string TABLE_NAME = "accounts.csv";

        private MongoDbContext mongoContext;

        public AccountsRepository(MongoDbContext context)
        {
            mongoContext = context;
        }

     

        public Account AddAcountRecord(Account account)
        {
            var nextId = GetNextAccountID();

            var accountEntity = new AccountEntity
            {
                AccountName = account.AccountName,
                AccountType = account.AccountType,
                CreatedDate = account.CreatedDate,
                Balance = account.Balance,
                OwnerUserId = account.OwnerUserId,
            };


            mongoContext.Accounts.InsertOne(accountEntity);

            // CSV: TODO: REMOVE

            var accounts = new List<AccountEntity>() { accountEntity };

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

            account.Id = accountEntity.Id;
            return account;

        }

        public Account GetOneAccountFromData(int accountId)
        {
            throw new NotImplementedException();
            //if (!File.Exists(TABLE_NAME))
            //{
            //    throw new DontExistException("Account table do not exist");
            //}

            //using (var reader = new StreamReader(TABLE_NAME))
            //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{
            //    var records = csv.GetRecords<AccountEntity>().ToList();

            //    if (records.Any())
            //    {
            //        var record = records.FirstOrDefault(x => x.Id == accountId);
            //        if (record != null)
            //        {
            //            var account = new Account()
            //            {
            //                Id = record.Id,
            //                AccountName = record.AccountName,
            //                AccountType = record.AccountType,
            //                Balance = record.Balance,
            //                CreatedDate = record.CreatedDate,
            //                OwnerUserId = record.OwnerUserId,
            //            };

            //            return account;
            //        }
            //        else
            //        {
            //            throw new NotFoundException("No account records found");
            //        }
            //    }
            //    else
            //    {
            //        throw new NotFoundException("No account records found");
            //    }
            //}
        }

        public List<Account> GetAllAccountsFromData()
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var results = csv.GetRecords<AccountEntity>().ToList();

                if (results.Any())
                {
                    var accountList = new List<Account>();

                    foreach (var record in results)
                    {
                        var account = new Account()
                        {
                            Id = record.Id,
                            AccountName = record.AccountName,
                            AccountType = record.AccountType,
                            Balance = record.Balance,
                            CreatedDate = record.CreatedDate,
                            OwnerUserId = record.OwnerUserId,
                        };

                        accountList.Add(account);
                    }

                    return accountList;
                }
                else
                {
                    throw new NotFoundException("No account records found");
                }
            }
        }

        public Account UpdateAccountRecord(UpdateAccount account)
        {
            throw new NotImplementedException();
            //if (!File.Exists(TABLE_NAME))
            //{
            //    throw new DontExistException("Account table do not exist");
            //}

            //var results = new List<AccountEntity>();

            //// получить все данные
            //using (var reader = new StreamReader(TABLE_NAME))
            //using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{
            //    results = csvReader.GetRecords<AccountEntity>().ToList();
            //}

            //if (results.Any())
            //{
            //    var record = results.FirstOrDefault(x => x.Id == account.Id);

            //    if (record != null)
            //    {
            //        record.AccountName = account.AccountName;
            //        record.UpdateDate = account.UpdateDate;

            //        var index = results.FindIndex(x => x.Id == account.Id);

            //        results[index] = record;

            //        using var writer = new StreamWriter(TABLE_NAME);
            //        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            //        {
            //            csvWriter.WriteRecords(results);

            //            var updatedAccount = new Account()
            //            {
            //                Id = record.Id,
            //                AccountName = record.AccountName,
            //                AccountType = record.AccountType,
            //                Balance = record.Balance,
            //                CreatedDate = record.CreatedDate,
            //                OwnerUserId = record.OwnerUserId,
            //                UpdateDate  = record.UpdateDate
            //            };

            //            return updatedAccount;
            //        }
            //    }
            //    else
            //    {
            //        throw new NotFoundException("No account records found");
            //    }
            //}
            //else
            //{
            //    throw new NotFoundException("No account records found");
            //}
        }

        public void DeleteAccountFromData(int accountId)
        {
            throw new NotImplementedException();
            //if (!File.Exists(TABLE_NAME))
            //{
            //    throw new DontExistException("Account table do not exist");
            //}

            //var records = new List<AccountEntity>();

            //using (var reader = new StreamReader(TABLE_NAME))
            //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{
            //    records = csv.GetRecords<AccountEntity>().ToList();
            //}

            //if (records.Any())
            //    {
            //        var index = records.FindIndex(x => x.Id == accountId);
            //        if (index != -1)
            //        {
            //            records.RemoveAt(index);

            //            using (var writer = new StreamWriter(TABLE_NAME))
            //            using (var csvToUpdate = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //            {
            //                csvToUpdate.WriteRecords(records);
            //            }

            //        }
            //        else
            //        {
            //            throw new NotFoundException("No account records found");
            //        }
            //    }
            //else
            //    {
            //        throw new NotFoundException("No account records found");
            //    }
        }

        private int GetNextAccountID()
        {
            if (!File.Exists("accountId.txt"))
            {
                File.WriteAllText("accountId.txt", "1");

                return 1;
            }
            else
            {
                var id = File.ReadAllText("accountId.txt");
                var intId = int.Parse(id);
                var nextId = intId + 1;
                File.WriteAllText("accountId.txt", nextId.ToString());
                return nextId;
            }

        }

        public Account GetByOwnerId(int ownerId)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<AccountEntity>().ToList();

                if (records.Any())
                {
                    var record = records.FirstOrDefault(x => x.OwnerUserId == ownerId);
                    if (record != null)
                    {
                        var account = new Account()
                        {
                            Id = record.Id,
                            AccountName = record.AccountName,
                            AccountType = record.AccountType,
                            Balance = record.Balance,
                            CreatedDate = record.CreatedDate,
                            OwnerUserId = record.OwnerUserId,
                        };

                        return account;
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

        public List <Account> GetAllAccountsByOwnerID(int ownerId)
        {
            if (!File.Exists(TABLE_NAME))
            {
                throw new DontExistException("Account table do not exist");
            }

            using (var reader = new StreamReader(TABLE_NAME))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<AccountEntity>().ToList();

                if (records.Any())
                {
                    var accountList = new List<Account>();
                    var filteredRecords = records.Where(x => x.OwnerUserId == ownerId).ToList();
                    if (filteredRecords != null)
                    {
                        foreach (var record in filteredRecords)
                        {
                            var account = new Account()
                            {
                                Id = record.Id,
                                AccountName = record.AccountName,
                                AccountType = record.AccountType,
                                Balance = record.Balance,
                                CreatedDate = record.CreatedDate,
                                OwnerUserId = record.OwnerUserId,
                            };

                            accountList.Add(account);
                        }

                            return accountList;
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




 