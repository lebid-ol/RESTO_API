using BankAccounts.Shared.Models;
using MediatR;
using static BankAccounts.Shared.Models.GenderType;

namespace BanksAccount.CQRS.Users.Commands.Create
{
       public class CreateUserCommand : IRequest<UserResponse>
    {


        public CreateUserCommand(string userName, string email, string userlastName,
            string phoneNumber, DateTime dateOfBirth, Gender gender, string billingAddress)
        {
            UserName = userName;
            Email = email;
            UserLastName = userlastName;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            BillingAddress = billingAddress;
        }


        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserLastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string BillingAddress { get; set; }
    }
}
