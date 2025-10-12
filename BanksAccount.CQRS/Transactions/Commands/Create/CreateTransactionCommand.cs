using MediatR;
using BankAccounts.API.Responses;                
using BankAccounts.Shared.Models;               

namespace BanksAccount.CQRS.Transactions.Commands.Create
{
    public sealed class CreateTransactionCommand : IRequest<TransactionResponse>
    {
        public CreateTransactionCommand(
            int accountId,
            string transactionName,
            string? description,
            decimal amountTransaction,
            TransactionType type)
        {
            AccountId = accountId;
            TransactionName = transactionName;
            Description = description;
            AmountTransaction = amountTransaction;
            Type = type;
        }

        public int AccountId { get; }
        public string TransactionName { get; }
        public string? Description { get; }
        public decimal AmountTransaction { get; }
        public TransactionType Type { get; }
    }
}
