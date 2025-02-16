namespace BankAccounts.Shared.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public int OwnerUserId { get; set; }
        public int Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
