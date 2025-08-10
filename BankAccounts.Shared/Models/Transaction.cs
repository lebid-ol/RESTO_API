using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccounts.Shared.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionName { get; set; }
        public string Description { get; set; }
        public decimal AmountTransaction { get; set; }
        public DateTime Created { get; set; }
        public int AccountId { get; set; }
        public TransactionType Type { get; set; }  // Credit / Debit

    }
}
