using BankAccounts.AppplicationData.Db;
using BanksAccount.CQRS.Accounts.Commands.Create; // AccountResponse
using MediatR;
using Microsoft.EntityFrameworkCore;
using BankAccounts.Exceptions;
using BanksAccount.CQRS.Accounts.Commands.Update;

namespace BanksAccount.CQRS.Accounts.Handlers
{
    public sealed class UpdateAccountCommandHandler
        : IRequestHandler<UpdateAccountCommand, AccountResponse>
    {
        private readonly PostgresDbContext _db;

        public UpdateAccountCommandHandler(PostgresDbContext db)
        {
            _db = db;
        }

        public async Task<AccountResponse> Handle(UpdateAccountCommand request, CancellationToken ct)
        {
            
            var entity = await _db.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

            if (entity is null)
                throw new NotFoundException("Account not found");

            entity.AccountName = request.AccountName.Trim();
            entity.UpdateDate = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

          
            return new AccountResponse
            {
                Id = entity.Id,
                AccountName = entity.AccountName,
                AccountType = entity.AccountType,
                Balance = entity.Balance,
                BalanceEuro = null,
                CreatedDate = entity.CreatedDate
            };
        }
    }
}

