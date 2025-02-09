namespace BankAccounts.Shared.Models.Request
{
    public class AccountRequest
    {
        public int OwnerUserId { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountName { get; set; }
    }
}
