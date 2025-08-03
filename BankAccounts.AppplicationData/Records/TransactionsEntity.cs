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
        public string TransactionName { get; set; }
        public string Description { get; set; }
        public int AmountTransaction { get; set; }
        public DateTime Created { get; set; }


        // Foreign key
        public int AccountId { get; set; }

        // Navigation property
        public AccountEntity Account { get; set; }
    }
}
