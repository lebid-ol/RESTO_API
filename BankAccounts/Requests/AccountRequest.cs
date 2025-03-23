using BankAccounts.Shared.Attributes;

namespace BankAccounts.Shared.Models.Request
{
    public class AccountRequest
    {
        public string OwnerUserId { get; set; }

        [EnumValidation(typeof(AccountType))]
        public AccountType AccountType { get; set; }
        public string AccountName { get; set; }
    }
}
