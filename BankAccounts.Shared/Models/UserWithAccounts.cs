using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccounts.Shared.Models
{
    public class UserWithAccounts
    {
        public User User { get; set; }
        public List<Account> Accounts
        {
            get; set;
        }
    }
}
