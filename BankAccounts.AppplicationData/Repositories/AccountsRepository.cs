using BankAccounts.AppplicationData.DbContext;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.Shared.Models;
using CsvHelper;
using CsvHelper.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace BankAccounts.Repositories
{
    public interface IAccountRepository
    {
        Account AddAcountRecord(Account accounts);
        Task<Account> GetOneAccountFromData(string accountId);
        Task<List<Account>> GetAllAccountsFromData();
        Account UpdateAccountRecord(UpdateAccount account);
        void DeleteAccountFromData(int accountId);
        Account GetByOwnerId(int ownerId);
        List <Account> GetAllAccountsByOwnerID(int ownerId);
    }

    public class AccountsRepository : IAccountRepository
    {
        private const string TABLE_NAME = "accounts.csv";

        private readonly MongoDbContext _mongoContext;

        public AccountsRepository(MongoDbContext context)
        {
            _mongoContext = context;
        }

        public Account AddAcountRecord(Account account)
        {
            var accountEntity = new AccountEntity
            {
                AccountName = account.AccountName,
                AccountType = account.AccountType,
                CreatedDate = account.CreatedDate,
                Balance = account.Balance,
                OwnerUserId = account.OwnerUserId,
            };

             _mongoContext.Accounts.InsertOne(accountEntity);

            account.Id = accountEntity.Id;
            return account;
        }

        public async Task<Account> GetOneAccountFromData(string accountId)
        {
            var taskResult = await _mongoContext.Accounts.FindAsync(x => x.Id == accountId);
            var accountEntity = taskResult.FirstOrDefault();

            if (accountEntity != null) 
            {
                var account = new Account()
                {
                    AccountName = accountEntity.AccountName,
                    AccountType = accountEntity.AccountType,
                    Balance = accountEntity.Balance,
                    CreatedDate = accountEntity.CreatedDate,
                    Id = accountEntity.Id,
                    OwnerUserId = accountEntity.OwnerUserId,
                    UpdateDate = accountEntity.UpdateDate
                };

                return account;
            }

            throw new NotFoundException("No account records found");
        }

        public async Task<List<Account>> GetAllAccountsFromData()
        {
            var documents = await _mongoContext.Accounts.Find(new BsonDocument()).ToListAsync();

            var accountList = new List<Account>();

            foreach (var record in documents)
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




 