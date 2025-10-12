using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Create;
using BanksAccount.CQRS.Users.Commands.Create;
using BanksAccount.CQRS.Users.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BanksAccount.CQRS.Users.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly PostgresDbContext _postgresDbContext;
        public GetUserByIdHandler(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }
        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userEntity = await _postgresDbContext.Users
                .Include(x => x.Accounts)
                .FirstOrDefaultAsync(x => x.Id == request.UserId);

            var accountList = new List<AccountResponse>();

            foreach (var record in userEntity.Accounts)
            {
                var account = new AccountResponse()
                {
                    Id = record.Id,
                    AccountName = record.AccountName,
                    AccountType = record.AccountType,
                    Balance = record.Balance,
                    CreatedDate = record.CreatedDate
                };

                accountList.Add(account);
            }

            if (userEntity != null)
            {
                var user = new UserResponse()
                {   
                    UserName = userEntity.UserName,
                    Email = userEntity.Email,
                    UserLastName = userEntity.UserLastName,
                    PhoneNumber = userEntity.PhoneNumber,
                    DateOfBirth = userEntity.DateOfBirth,
                    BillingAddress = userEntity.BillingAddress,
                    Accounts = accountList
                };

                return user;
            }

            throw new NotFoundException("No users records found");
        }
    }
}
