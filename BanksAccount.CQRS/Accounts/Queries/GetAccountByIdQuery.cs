using BanksAccount.CQRS.Accounts.Commands.Create;
using MediatR;

namespace BanksAccount.CQRS.Accounts.Queries
{
    public class GetAccountByIdQuery : IRequest<AccountResponse>
    {
        public int AccountId { get; set; }

        public GetAccountByIdQuery(int accountId)
        {
            AccountId = accountId;
        }
    }
}
