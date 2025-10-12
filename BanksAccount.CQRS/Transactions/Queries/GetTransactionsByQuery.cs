using MediatR;
using BankAccounts.API.Responses;


namespace BanksAccount.CQRS.Transactions.Queries
{
    public  record GetTransactionsByQuery(
         int AccountId,
         DateOnly? From,
         DateOnly? To
     ) : IRequest<List<TransactionResponse>>;
}

