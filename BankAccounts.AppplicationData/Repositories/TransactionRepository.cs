using BankAccounts.AppplicationData.Db;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankAccounts.Repositories
{

    public interface ITransactionRepository
    {
        Task<Transaction> AddTransactionRecord(Transaction transaction);
        Task<List<Transaction>> GetTransactionsByAccountFromDb(int accountId);

    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly PostgresDbContext _postgresDbContext;      


        public TransactionRepository(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<Transaction> AddTransactionRecord(Transaction transaction)
        {
            var transactionEntity = new TransactionsEntity
            {
                TransactionName = transaction.TransactionName,
                Description = transaction.Description,
                AmountTransaction = transaction.AmountTransaction,
                Created = transaction.Created,
                Id = transaction.Id

            };

            _postgresDbContext.Transactions.Add(transactionEntity);
            await _postgresDbContext.SaveChangesAsync();

            transaction.Id = transactionEntity.Id;
            return transaction;
        }

        public async Task<List<Transaction>> GetTransactionsByAccountFromDb(int accountId)
        {
            return await _postgresDbContext.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == accountId)
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
