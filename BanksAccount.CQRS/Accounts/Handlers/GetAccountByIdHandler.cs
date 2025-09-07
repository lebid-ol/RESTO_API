using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Create;
using BanksAccount.CQRS.Accounts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BanksAccount.CQRS.Accounts.Handlers
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountResponse>
    {
        private readonly PostgresDbContext _postgresDbContext;

        public GetAccountByIdHandler(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<AccountResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var accountEntity = await _postgresDbContext.Accounts
               .Where(a => a.Id == request.AccountId)
               .Select(a => new Account
               {
                   AccountName = a.AccountName,
                   AccountType = a.AccountType,
                   Balance = a.Balance,
                   CreatedDate = a.CreatedDate,
                   Id = a.Id,
                   UpdateDate = a.UpdateDate,
                   TransactionList = a.Transactions
                      .OrderByDescending(record => record.Created)
                      .ThenByDescending(record => record.Id) // на случай одинаковой даты
                      .Take(10)
                      .Select(record => new Transaction
                      {
                          Id = record.Id,
                          TransactionName = record.TransactionName,
                          Description = record.Description,
                          AmountTransaction = record.AmountTransaction,
                          Created = record.Created
                      })
                      .ToList()
               })

                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (accountEntity is null)
            {

                throw new NotFoundException("No account records found");

            }

            var response = new AccountResponse()
            {
                AccountName = accountEntity.AccountName,
                AccountType = accountEntity.AccountType,
                Balance = accountEntity.Balance,
                Id = accountEntity.Id,
            };

            return response;
        }
    }
}
