using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Delete;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BanksAccount.CQRS.Accounts.Handlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand>
    {
        private readonly PostgresDbContext _postgresDbContext;
        private readonly ILogger<CreateAccountHandler> _logger;

        public DeleteAccountHandler(PostgresDbContext postgresDbContext, ILogger<CreateAccountHandler> logger)
        {
            _postgresDbContext = postgresDbContext;
            _logger = logger;
        }

        public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var accountToDelete = await _postgresDbContext.Accounts.FindAsync(request.AccountId);

            if (accountToDelete == null)
            {
                throw new NotFoundException("Account not found");
            }

            _postgresDbContext.Accounts.Remove(accountToDelete);

            await _postgresDbContext.SaveChangesAsync();

            _logger.LogInformation($"Account witb ID {request.AccountId} succesfully  deleted");
        }
    }
}
