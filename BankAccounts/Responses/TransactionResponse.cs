using BankAccounts.Shared.Models;

namespace BankAccounts.API.Responses
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public string TransactionName { get; set; }
        public string Description { get; set; }
        public decimal AmountTransaction { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
