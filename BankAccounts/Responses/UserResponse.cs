using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAccounts.ResponseModels;
using static BankAccounts.Shared.Models.GenderType;

namespace BankAccounts.Shared.Models.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public string UserLastName { get; set; } 
        public string PhoneNumber { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public Gender Gender { get; set; } 
        public string BillingAddress { get; set; }
        public IEnumerable<AccountResponse> Accounts { get; set; } = new List<AccountResponse>();
    }
}
