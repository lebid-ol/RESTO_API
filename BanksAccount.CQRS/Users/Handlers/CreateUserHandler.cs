using MediatR;
using BankAccounts.AppplicationData.Db;
using BankAccounts.AppplicationData.Records;
using BanksAccount.CQRS.Users.Commands.Create;

namespace BanksAccount.CQRS.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly PostgresDbContext _postgresDbContext;

        public CreateUserHandler(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity
            {
                UserName = request.UserName,
                Email = request.Email,
                UserLastName = request.UserLastName,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth.ToUniversalTime(),
                BillingAddress = request.BillingAddress,
                Gender = request.Gender
            };


            await  _postgresDbContext.Users.AddAsync(userEntity, cancellationToken);

            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            var response = new UserResponse()
            {
                UserName = userEntity.UserName,
                Id = userEntity.Id,
                Email = userEntity.Email,
                UserLastName = userEntity.UserLastName,
                PhoneNumber = userEntity.PhoneNumber,
                DateOfBirth = userEntity.DateOfBirth,
                Gender = userEntity.Gender,
                BillingAddress = userEntity.BillingAddress,

            };


             response.Id = userEntity.Id;

             return response;
         
        }
    }
    }

