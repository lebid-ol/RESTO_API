using BankAccounts.API.Responses;
using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BanksAccount.CQRS.Transactions.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace BanksAccount.CQRS.Transactions.Handlers
{
    public  class GetTransactionsHandler
         : IRequestHandler<GetTransactionsByQuery, List<TransactionResponse>>
    {
        private readonly PostgresDbContext _db;

        public GetTransactionsHandler(PostgresDbContext db) => _db = db;

        public async Task<List<TransactionResponse>> Handle(GetTransactionsByQuery request, CancellationToken ct)
        {
            if (request.AccountId <= 0)
                throw new ArgumentException("Invalid account id.", nameof(request.AccountId));

            // Убедимся, что счёт существует (и вернём 404, а не пустой список, если его нет)
            var accountExists = await _db.Accounts
                .AsNoTracking()
                .AnyAsync(a => a.Id == request.AccountId, ct);

            if (!accountExists)
                throw new NotFoundException($"Account {request.AccountId} not found");

            var q = _db.Transactions
                .AsNoTracking()
                .Where(t => t.AccountId == request.AccountId);

            // from: включительно, начало дня (UTC)
            if (request.From.HasValue)
            {
                var fromInclusiveUtc = request.From.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                q = q.Where(t => t.Created >= fromInclusiveUtc);
            }

            // to: эксклюзивно, (дата_to + 1 день 00:00 UTC)
            if (request.To.HasValue)
            {
                var toExclusiveUtc = request.To.Value.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                q = q.Where(t => t.Created < toExclusiveUtc);
            }

            var list = await q
                .OrderByDescending(t => t.Created)
                .Select(e => new TransactionResponse
                {
                    Id = e.Id,
                    TransactionName = e.TransactionName!,
                    Description = e.Description!,
                    AmountTransaction = e.AmountTransaction, // уже со знаком (+ кредит, − дебет)
                    Created = e.Created
                })
                .ToListAsync(ct);

            return list;
        }
    }

}
