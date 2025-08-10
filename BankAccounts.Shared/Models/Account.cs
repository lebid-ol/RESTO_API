namespace BankAccounts.Shared.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal? BalanceInEuro { get; set; }
        public List<Transaction> TransactionList { get; set; }
    }
}
