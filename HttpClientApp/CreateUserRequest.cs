using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientApp
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string BillingAddress { get; set; }
        public string DateOfBirth { get; set; }
    }

}
