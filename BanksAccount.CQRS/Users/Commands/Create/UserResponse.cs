using BankAccounts.Shared.Models;
using BanksAccount.CQRS.Accounts.Commands.Create;
using static BankAccounts.Shared.Models.GenderType;

namespace BanksAccount.CQRS.Users.Commands.Create
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public string UserLastName { get; set; } 
        public string PhoneNumber { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public Gender Gender { get; set; } 
        public string BillingAddress { get; set; }
        public IEnumerable<AccountResponse> Accounts { get; set; } = new List<AccountResponse>();
    }
}
