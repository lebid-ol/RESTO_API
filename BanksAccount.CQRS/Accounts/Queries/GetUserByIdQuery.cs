using BanksAccount.CQRS.Accounts.Commands.Create;
using BanksAccount.CQRS.Users.Commands.Create;
using MediatR; 


namespace BanksAccount.CQRS.Accounts.Queries
{
    public class GetUserByIdQuery : IRequest<UserResponse>
    {
        public int UserId { get; set; }
        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
