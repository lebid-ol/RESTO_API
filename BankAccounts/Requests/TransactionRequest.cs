using BankAccounts.Shared.Models;

namespace BankAccounts.API.Requests
{
    public class TransactionRequest
    {
        public int AccountId { get; set; }
        public string TransactionName { get; set; } = default!;
        public string? Description { get; set; }
        public decimal AmountTransaction { get; set; }
        public TransactionType  Type { get; set; }  // Credit / Debit
    }
       
}
