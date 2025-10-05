using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Delete;
using MediatR;

namespace BanksAccount.CQRS.Accounts.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly PostgresDbContext _postgresDbContext;
        public DeleteUserHandler(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userToDelete = await _postgresDbContext.Users.FindAsync(request.UserId);
            if (userToDelete == null)
            {
                throw new NotFoundException("User not found");
            }
            _postgresDbContext.Users.Remove(userToDelete);
            await _postgresDbContext.SaveChangesAsync();
        }
    }
}
