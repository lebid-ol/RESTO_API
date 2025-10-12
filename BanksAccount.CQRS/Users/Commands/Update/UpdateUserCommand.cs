using BanksAccount.CQRS.Users.Commands.Create;
using MediatR;




namespace BanksAccount.CQRS.Users.Commands.Update
{
    public sealed class UpdateUserCommand : IRequest<UserResponse>
    {
        public UpdateUserCommand(
            int id,
            string userName,
            string email,
            string userLastName,
            string phoneNumber,
            DateTime dateOfBirth,
            string billingAddress)
        {
            Id = id;
            UserName = userName;
            Email = email;
            UserLastName = userLastName;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            BillingAddress = billingAddress;
        }

        public int Id { get; }
        public string UserName { get; }
        public string Email { get; }
        public string UserLastName { get; }
        public string PhoneNumber { get; }
        public DateTime DateOfBirth { get; }
        public string BillingAddress { get; }
    }
}
