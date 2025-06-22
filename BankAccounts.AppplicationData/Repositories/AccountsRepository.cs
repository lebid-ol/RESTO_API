using BankAccounts.AppplicationData.DbContext;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.Shared.Clients.CurrencyConver;
using BankAccounts.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;




namespace BankAccounts.Repositories
{
    public interface IAccountRepository
    {
        Account AddAcountRecord(Account accounts);
        Task<Account> GetOneAccountFromData(string accountId);
        Task<List<Account>> GetAllAccountsFromData();
        Task<Account> UpdateAccountRecord(UpdateAccount account);
        Task DeleteAccountFromData(string accountId);
        Task<List<Account>> GetAllAccountsByOwnerId(string ownerId);
    }

    public class AccountsRepository : IAccountRepository
    {
        private readonly MongoDbContext _mongoContext;
        private readonly ICurrencyConverterClient _currencyConverter;


        public AccountsRepository(IConfiguration configuration, MongoDbContext context, IOptions<AzureSettingsOptions> options, ICurrencyConverterClient currencyConverter)
        {
            var appName = configuration["AppName"];
            var mongoOptions = options.Value;
            _mongoContext = context;
            _currencyConverter = currencyConverter;
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

        public async Task<Account> UpdateAccountRecord(UpdateAccount account)
        {
            var filter = Builders<AccountEntity>.Filter.Eq(x => x.Id, account.Id);

            var update = Builders<AccountEntity>.Update
                .Set(x => x.AccountName, account.AccountName)
                .Set(x => x.UpdateDate, account.UpdateDate);

            var updateResult = await _mongoContext.Accounts.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount == 1)
            {
                var taskResult = await _mongoContext.Accounts.FindAsync(x => x.Id == account.Id);
                var accountEntity = taskResult.FirstOrDefault();

                if (accountEntity != null)
                {
                    var accountUpdate = new Account()
                    {
                        AccountName = accountEntity.AccountName,
                        AccountType = accountEntity.AccountType,
                        Balance = accountEntity.Balance,
                        CreatedDate = accountEntity.CreatedDate,
                        Id = accountEntity.Id,
                        OwnerUserId = accountEntity.OwnerUserId,
                        UpdateDate = accountEntity.UpdateDate
                    };


                    return accountUpdate;
                }
            }

            if (updateResult.ModifiedCount > 1)
            {
                Console.WriteLine("MEssage to developer");
                throw new Exception("Developer exception");
            }

            throw new NotFoundException("No account records found");
        }

        public async Task DeleteAccountFromData(string accountId)
        {
            var  deleteResult = await _mongoContext.Accounts.DeleteOneAsync(x => x.Id == accountId);

            if (deleteResult.DeletedCount == 1)
            {
                return;
            }

            if (deleteResult.DeletedCount > 1)
            {
                Console.WriteLine("MEssage to developer");
                throw new Exception("Developer exception");
            }

            throw new NotFoundException("No account records found");
        }

        public async Task<List<Account>> GetAllAccountsByOwnerId(string ownerId)
        {
            var taskResult = await _mongoContext.Accounts.FindAsync(x => x.OwnerUserId == ownerId);
            var accountEntity = taskResult.ToList();

            if (accountEntity.Any())
            {
                var result = new List<Account>();

                foreach (var item in accountEntity)
                {
                    var account = new Account()
                    {
                        AccountName = item.AccountName,
                        AccountType = item.AccountType,
                        Balance = item.Balance,
                        CreatedDate = item.CreatedDate,
                        Id = item.Id,
                        OwnerUserId = item.OwnerUserId,
                        UpdateDate = item.UpdateDate
                    };


                    result.Add(account);
                }

                return result;
            }

            throw new NotFoundException("No account records found");
        }
    }
}




 