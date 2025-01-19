namespace BankAccounts.Records
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public int Balance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
