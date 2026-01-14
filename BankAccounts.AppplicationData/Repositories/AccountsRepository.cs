using BankAccounts.AppplicationData.Db;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;



namespace BankAccounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> AddAcountRecord(Account accounts);
        Task<Account> GetOneAccountFromData(int accountId);
        Task<List<Account>> GetAllAccountsFromData();
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
                UpdateDate = account.UpdateDate,
                UserId = account.UserId,
     
            };

            _postgresDbContext.Accounts.Add(accountEntity);
            await _postgresDbContext.SaveChangesAsync();

            account.Id = accountEntity.Id;
            return account;
        }

        public async Task<Account> GetOneAccountFromData(int accountId)
        {
            var accountEntity = await _postgresDbContext.Accounts
               .Where(a => a.Id == accountId)
               .Select(a => new Account
               {
                   AccountName = a.AccountName,
                   AccountType = a.AccountType,
                   Balance = a.Balance,
                   CreatedDate = a.CreatedDate,
                   Id = a.Id,
                   UpdateDate = a.UpdateDate,
                   TransactionList = a.Transactions
                      .OrderByDescending(record => record.Created)
                      .ThenByDescending(record => record.Id) // на случай одинаковой даты
                      .Take(10)
                      .Select(record => new Transaction
                      {
                          Id = record.Id,
                          TransactionName = record.TransactionName,
                          Description = record.Description,
                          AmountTransaction = record.AmountTransaction,
                          Created = record.Created
                      })
                      .ToList()
               })

                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (accountEntity is null) 
            {

                throw new NotFoundException("No account records found");
                
            }

            return accountEntity;
        }

        public async Task<List<Account>> GetAllAccountsFromData()
        {
            var accounts = await _postgresDbContext.Accounts.ToListAsync();

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




 