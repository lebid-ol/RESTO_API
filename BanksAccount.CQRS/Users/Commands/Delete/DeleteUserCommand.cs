using MediatR;


namespace BanksAccount.CQRS.Users.Commands.Delete
{
    public  class DeleteUserCommand : IRequest
    {
        public int UserId { get; set; }
        public DeleteUserCommand(int userId)
        {
            UserId = userId;
        }
    }
}
