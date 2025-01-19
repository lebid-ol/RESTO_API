using BankAccounts.Models;

namespace BankAccounts.Shared.Models.Request
{
    public class AccountRequest
    {
        public string AccountName { get; set; }
        public string AccountHolderName { get; set; }
        public string Email { get; set; }
        public string AccountHolderLastName { get; set; }
        public AccountType AccountType { get; set; }
    }
}
