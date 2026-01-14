using BankAccounts.AppplicationData.Db;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;
using BankAccounts.Repositories;
using BankAccounts.Shared.Exceptions;
using BankAccounts.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankAccounts.ApplicationService.Services
{

    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(Transaction tx, Account account, TransactionType type);
        Task<List<Transaction>> GetTransactionsByAccount(int accountId, DateOnly? from = null, DateOnly? to = null);

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

            var transaction = await _transactionRepository.AddTransactionRecord(tx,account,type);

            return transaction;
        }

        public async Task<List<Transaction>> GetTransactionsByAccount(int accountId, DateOnly? from = null, DateOnly? to = null)
        {
            return await _transactionRepository.GetTransactionsByAccountFromDb(accountId, from, to);
        }
    }
}



   