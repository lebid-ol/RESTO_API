using BankAccounts.AppplicationData.Db;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.Repositories;
using BankAccounts.Shared.Exceptions;
using BankAccounts.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.ApplicationService.Services
{

    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(Transaction tx, Account account, TransactionType type);
        Task<List<Transaction>> GetTransactionsByAccount(int accountId);

    }
    public class TransactionService : ITransactionService
    {
        private readonly PostgresDbContext _db;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(PostgresDbContext db, ITransactionRepository transactionRepository)
        {
            _db = db;
            _transactionRepository = transactionRepository;
        }


        public async Task<Transaction> AddTransaction(Transaction tx, Account account, TransactionType type)
        {
            if (tx.AmountTransaction <= 0) throw new DomainException("Amount must be positive.");

            await using var dbtx = await _db.Database.BeginTransactionAsync();

            try
            {
                // 1) Берём отслеживаемую сущность аккаунта
                var accEntity = await _db.Accounts
                    .FirstOrDefaultAsync(a => a.Id == account.Id); // account.Id пришёл извне
                if (accEntity is null)
                    throw new NotFoundException($"Account {account.Id} not found.");

                decimal delta = type == TransactionType.Debit ? -tx.AmountTransaction : tx.AmountTransaction;

                accEntity.Balance += delta;
                if (accEntity.Balance < 0)
                    throw new DomainException("Insufficient funds.");

                // ВАЖНО: если загрузили через этот же DbContext, вызывать Update() не нужно —
                // EF уже отслеживает изменения. Достаточно SaveChangesAsync().

                // 2) Пишем транзакцию (связываем по FK)
                var txEntity = new TransactionsEntity
                {
                    AccountId = accEntity.Id,
                    TransactionName = tx.TransactionName,
                    Description = tx.Description,
                    AmountTransaction = delta,            // decimal, со знаком
                    Created = DateTime.UtcNow
                };
                _db.Transactions.Add(txEntity);

                await _db.SaveChangesAsync();
                await dbtx.CommitAsync();

                // маппинг обратно, если нужен доменный объект:
                return new Transaction
                {
                    Id = txEntity.Id,
                    AccountId = txEntity.AccountId,
                    TransactionName = txEntity.TransactionName,
                    Description = txEntity.Description,
                    AmountTransaction = txEntity.AmountTransaction,
                    Created = txEntity.Created
                };
            }
            catch
            {
                await dbtx.RollbackAsync();
                throw;
            }

        }

       
        public async Task<List<Transaction>> GetTransactionsByAccount(int accountId)
        {
            return await _transactionRepository.GetTransactionsByAccountFromDb(accountId);
        }
    }
}



   