using BankAccounts.AppplicationData.Db;
using BankAccounts.Records;
using BankAccounts.Shared.Clients.CurrencyConver;
using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Create;
using MediatR;

namespace BanksAccount.CQRS.Accounts.Handlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountResponse>
    {
        private readonly PostgresDbContext _postgresDbContext;

        public CreateAccountHandler(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<AccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var accountEntity = new AccountEntity
            {
                AccountName = request.AccountName,
                AccountType = request.AccountType,
                CreatedDate = DateTime.UtcNow,
                Balance = 100,
                UpdateDate = DateTime.UtcNow,
                UserId = request.UserId,

            };

            _postgresDbContext.Accounts.Add(accountEntity);

            await _postgresDbContext.SaveChangesAsync();

            var response = new AccountResponse()
            {
                AccountName = accountEntity.AccountName,
                Id = accountEntity.Id,
                AccountType = accountEntity.AccountType,
                Balance = accountEntity.Balance,
            };

            return response;
        }
    }
}
