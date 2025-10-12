using BankAccounts.AppplicationData.Db;
using BankAccounts.Exceptions;
using BankAccounts.Records;
using BanksAccount.CQRS.Accounts.Commands.Create;
using BanksAccount.CQRS.Users.Commands.Create;
using BanksAccount.CQRS.Users.Commands.Update;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BanksAccount.CQRS.Users.Handlers
{
    public class UpdateUserHandler
        : IRequestHandler<UpdateUserCommand, UserResponse>
    {
        private readonly PostgresDbContext _db;

        public UpdateUserHandler(PostgresDbContext db)
        {
            _db = db;
        }

        public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken ct)
        {

            var entity = await _db.Users
               .Include(u => u.Accounts)
               .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

            if (entity is null)
                throw new NotFoundException("No users records found");


            // Нормализация ввода
            entity.UserName = request.UserName?.Trim();
            entity.Email = request.Email?.Trim();
            entity.UserLastName = request.UserLastName?.Trim();
            entity.PhoneNumber = request.PhoneNumber?.Trim();
            entity.BillingAddress = request.BillingAddress?.Trim();

            // Дата рождения: храним в UTC 
            entity.DateOfBirth = request.DateOfBirth.Kind switch
            {
                DateTimeKind.Utc => request.DateOfBirth,
                DateTimeKind.Unspecified => DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
                _ => request.DateOfBirth.ToUniversalTime()
            };

            await _db.SaveChangesAsync(ct);

            // Маппим аккаунты в DTO
            var accountDtos = (entity.Accounts ?? new List<AccountEntity>())
                   .OrderByDescending(a => a.CreatedDate)
                   .Select(a => new AccountResponse
                   {
                     Id = a.Id,
                     AccountName = a.AccountName,
                     AccountType = a.AccountType,
                     Balance = a.Balance,
                     BalanceEuro = null,
                     CreatedDate = a.CreatedDate
                   })
                   .ToList();

            return new UserResponse
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                UserLastName = entity.UserLastName,
                PhoneNumber = entity.PhoneNumber,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                BillingAddress = entity.BillingAddress,
                Accounts = accountDtos
            };
        }
    }
}
