using MediatR;


namespace BanksAccount.CQRS.Accounts.Commands.Delete
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
