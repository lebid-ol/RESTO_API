using BankAccounts.API.Responses;
using BankAccounts.RequestModel;
using BankAccounts.Shared.Models;

namespace BankAccounts.ResponseModels
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public decimal? BalanceEuro { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}
