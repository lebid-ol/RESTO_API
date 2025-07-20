using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.Shared.Models;
using MongoDB.Driver;




namespace BankAccounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> AddAcountRecord(Account accounts);
        Task<Account> GetOneAccountFromData(int accountId);
        List<Account> GetAllAccountsFromData();
        Task<Account> UpdateAccountRecord(UpdateAccount account);
        Task DeleteAccountFromData(int accountId);
        Task<List<Account>> GetAllAccountsByOwnerId(int ownerId);
    }

    public class AccountsRepository : IAccountRepository
    {
        private readonly PostgresDbContext _postgresDbContext;


        public AccountsRepository(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<Account> AddAcountRecord(Account account)
        {
            var accountEntity = new AccountEntity
            {
                AccountName = account.AccountName,
                AccountType = account.AccountType,
                CreatedDate = account.CreatedDate,
                Balance = account.Balance,
                UpdateDate = account.UpdateDate
     
            };

            _postgresDbContext.Accounts.Add(accountEntity);
            await _postgresDbContext.SaveChangesAsync();

            account.Id = accountEntity.Id;
            return account;
        }

        public async Task<Account> GetOneAccountFromData(int accountId)
        {
            var accountEntity = await _postgresDbContext.Accounts.FindAsync(accountId);

            if (accountEntity != null) 
            {
                var account = new Account()
                {
                    AccountName = accountEntity.AccountName,
                    AccountType = accountEntity.AccountType,
                    Balance = accountEntity.Balance,
                    CreatedDate = accountEntity.CreatedDate,
                    Id = accountEntity.Id,
                    UpdateDate = accountEntity.UpdateDate
                };

                return account;
            }

            throw new NotFoundException("No account records found");
        }

        public  List<Account> GetAllAccountsFromData()
        {
            var accounts = _postgresDbContext.Accounts.ToList();

            var accountList = new List<Account>();

            foreach (var record in accounts)
            {
                var account = new Account()
                {
                    Id = record.Id,
                    AccountName = record.AccountName,
                    AccountType = record.AccountType,
                    Balance = record.Balance,
                    CreatedDate = record.CreatedDate,
                };

                accountList.Add(account);
            }

            return accountList;
        }

        public async Task<Account> UpdateAccountRecord(UpdateAccount account)
        {
            var accountToUpdate = await _postgresDbContext.Accounts.FindAsync(account.Id);

            if (accountToUpdate == null)
            {
                throw new NotFoundException("Account not found");
            }

            accountToUpdate.AccountName = account.AccountName;
            accountToUpdate.UpdateDate = account.UpdateDate;

            await _postgresDbContext.SaveChangesAsync();

            return new Account
            {
                AccountName = accountToUpdate.AccountName,
                AccountType = accountToUpdate.AccountType,
                Balance = accountToUpdate.Balance,
                CreatedDate = accountToUpdate.CreatedDate,
                Id = accountToUpdate.Id,
                UpdateDate = accountToUpdate.UpdateDate
            };
        }

        public async Task DeleteAccountFromData(int accountId)
        {
            var accountToDelete = await _postgresDbContext.Accounts.FindAsync(accountId);

            if (accountToDelete == null) 
            {
                throw new NotFoundException("Account not found");
            }

            _postgresDbContext.Accounts.Remove(accountToDelete);

            await _postgresDbContext.SaveChangesAsync();
        }

        public async Task<List<Account>> GetAllAccountsByOwnerId(int ownerId)
        {
            throw new NotImplementedException();
        }
    }
}




 