using MediatR;
using Microsoft.EntityFrameworkCore;
using BankAccounts.API.Responses;
using BankAccounts.AppplicationData.Db;
using BankAccounts.AppplicationData.Records;
using BankAccounts.Exceptions;        
using BankAccounts.Shared.Exceptions; 
using BankAccounts.Shared.Models;    

namespace BanksAccount.CQRS.Transactions.Commands.Create
{
    public class CreateTransactionHandler
        : IRequestHandler<CreateTransactionCommand, TransactionResponse>
    {
        private readonly PostgresDbContext _db;

        public CreateTransactionHandler(PostgresDbContext db) => _db = db;

        public async Task<TransactionResponse> Handle(CreateTransactionCommand request, CancellationToken ct)
        {
            if (request.AmountTransaction <= 0)
                throw new DomainException("Amount must be positive.");
            if (string.IsNullOrWhiteSpace(request.TransactionName))
                throw new DomainException("TransactionName is required.");

            // Начинаем БД-транзакцию
            await using var dbtx = await _db.Database.BeginTransactionAsync(ct);

            try
            {
                // 1) Находим счёт (отслеживаемая сущность)
                var account = await _db.Accounts
                    .FirstOrDefaultAsync(a => a.Id == request.AccountId, ct);

                if (account is null)
                    throw new NotFoundException($"Account {request.AccountId} not found.");

                // 2) Считаем дельту по типу операции
                var delta = request.Type == TransactionType.Debit
                    ? -request.AmountTransaction
                    : request.AmountTransaction;

                // 3) Обновляем баланс
                account.Balance += delta;
                if (account.Balance < 0)
                    throw new DomainException("Insufficient funds.");

                // 4) Пишем транзакцию (FK на счёт)
                var nowUtc = DateTime.UtcNow;
                var txEntity = new TransactionsEntity
                {
                    AccountId = account.Id,
                    TransactionName = request.TransactionName.Trim(),
                    Description = request.Description?.Trim(),
                    AmountTransaction = delta,           // храним со знаком
                    Type = request.Type,
                    Created = nowUtc
                };

                _db.Transactions.Add(txEntity);

                // 5) Фиксируем
                await _db.SaveChangesAsync(ct);
                await dbtx.CommitAsync(ct);

                // 6) Ответ DTO
                return new TransactionResponse
                {
                    Id = txEntity.Id,
                    TransactionName = txEntity.TransactionName!,
                    Description = txEntity.Description!,
                    AmountTransaction = txEntity.AmountTransaction,
                    Created = txEntity.Created
                };
            }
            catch
            {
                await dbtx.RollbackAsync(ct);
                throw;
            }
        }
    }
}
