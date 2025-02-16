using BankAccounts.RequestModel;
using BankAccounts.Shared.Models;

namespace BankAccounts.ResponseModels
{
    public class AccountResponse
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public int Balance { get; set; }
    }
}
