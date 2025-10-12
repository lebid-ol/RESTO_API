using MediatR;
using BanksAccount.CQRS.Accounts.Commands.Create;
using BankAccounts.Shared.Models;


namespace BanksAccount.CQRS.Accounts.Commands.Update
    {
        public class UpdateAccountCommand : IRequest<AccountResponse>
        {
            public UpdateAccountCommand(int id, string accountName)
            {
                Id = id;
                AccountName = accountName;
            }

            public int Id { get; }
            public string AccountName { get; }
        }
    }



