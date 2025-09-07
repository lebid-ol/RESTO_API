using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Delete;
using MediatR;

namespace BanksAccount.CQRS.Accounts.Handlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand>
    {
        private readonly PostgresDbContext _postgresDbContext;

        public DeleteAccountHandler(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
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
        }
    }
}
