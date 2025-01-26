using BankAccounts.Models;
using BankAccounts.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankAccounts.Shared.Models.GenderType;

namespace BankAccounts.Shared.Models.Requests
{
    public class UserRequest     {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserLastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string BillingAddress { get; set; }
    }

    
}
