using BankAccounts.Records;
using BankAccounts.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAccounts.AppplicationData.Records;


namespace BankAccounts.AppplicationData.Records
{
    public class TransactionsEntity
    {
        public int Id { get; set; }
        public string TransactionName { get; set; } = default!;
        public string? Description { get; set; }
        public decimal AmountTransaction { get; set; }
        public DateTime Created { get; set; }
        public TransactionType Type { get; set; }


        // Foreign key
        public int AccountId { get; set; }

        // Navigation property
        public AccountEntity Account { get; set; } = default!;
    }
}
