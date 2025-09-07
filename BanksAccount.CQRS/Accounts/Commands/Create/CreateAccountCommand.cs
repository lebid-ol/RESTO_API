using BankAccounts.Shared.Models;
using MediatR;

namespace BanksAccount.CQRS.Accounts.Commands.Create
{
    public class CreateAccountCommand : IRequest<AccountResponse>
    {
        public CreateAccountCommand(string accountName, AccountType accountType, int userId)
        {
            AccountName = accountName;
            AccountType = accountType;
            UserId = userId;
        }

        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public int UserId { get; set; }
    }
}
