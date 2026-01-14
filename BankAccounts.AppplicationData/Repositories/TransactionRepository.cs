using BankAccounts.AppplicationData.Db;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.Shared.Exceptions;
using BankAccounts.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankAccounts.Repositories
{

    public interface ITransactionRepository
    {
        Task<Transaction> AddTransactionRecord(Transaction transaction, Account account, TransactionType type);
        Task<List<Transaction>> GetTransactionsByAccountFromDb(int accountId, DateOnly? from = null, DateOnly? to = null);

    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly PostgresDbContext _postgresDbContext;      


        public TransactionRepository(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<Transaction> AddTransactionRecord(Transaction tx, Account account, TransactionType type)
        {
            await using var dbtx = await _postgresDbContext.Database.BeginTransactionAsync();

            try
            {
                // 1) Берём отслеживаемую сущность аккаунта
                var accEntity = await _postgresDbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Id == account.Id); // account.Id пришёл извне
                if (accEntity is null)
                    throw new NotFoundException($"Account {account.Id} not found.");

                decimal delta = type == TransactionType.Debit 
                    ? -tx.AmountTransaction 
                    : tx.AmountTransaction;

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
                _postgresDbContext.Transactions.Add(txEntity);

                await _postgresDbContext.SaveChangesAsync();
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
        public async Task<List<Transaction>> GetTransactionsByAccountFromDb(int accountId, DateOnly? from = null, DateOnly? to = null)
        {
            var q = _postgresDbContext.Transactions
                .AsNoTracking()
                .Where(t => t.AccountId == accountId);

            if (from.HasValue)
            {
                // считаем, что в БД Created хранится в UTC
                var fromInclusiveUtc = from.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                q = q.Where(t => t.Created >= fromInclusiveUtc);
            }

            if (to.HasValue)
            {
                // правая граница — эксклюзивная: дата_to + 1 день, 00:00
                var toExclusiveUtc = to.Value.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                q = q.Where(t => t.Created < toExclusiveUtc);
            }

            return await q
                .OrderByDescending(t => t.Created)
                .Select(e => new Transaction
                {
                    Id = e.Id,
                    TransactionName = e.TransactionName,
                    Description = e.Description,
                    AmountTransaction = e.AmountTransaction,
                    Created = e.Created,
                    AccountId = e.AccountId,
                    Type = e.Type
                })
                .ToListAsync();
        }
    }
    }
