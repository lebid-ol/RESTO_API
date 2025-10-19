using BankAccounts.AppplicationData.Db;
using BankAccounts.Records;
using BankAccounts.Shared.Clients.CurrencyConver;
using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Create;
using DnsClient.Internal;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BanksAccount.CQRS.Accounts.Handlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountResponse>
    {
        private readonly PostgresDbContext _postgresDbContext;
        private readonly ILogger<CreateAccountHandler> _logger;

        public CreateAccountHandler(PostgresDbContext postgresDbContext,ILogger <CreateAccountHandler> logger)
        {
            _postgresDbContext = postgresDbContext;
            _logger = logger;
        }

        public async Task<AccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating  account with name {request.AccountName}");

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

            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            var response = new AccountResponse()
            {
                AccountName = accountEntity.AccountName,
                Id = accountEntity.Id,
                AccountType = accountEntity.AccountType,
                Balance = accountEntity.Balance,
            };

            _logger.LogInformation($"Succesfully create  account with ID {accountEntity.Id}");

            return response;
        }
    }
}
