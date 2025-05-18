using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientApp
{
    public class BankAccountResponse
    {
        public string id { get; set; }
        public string accountName { get; set; }
        public string accountType { get; set; }
        public int balance { get; set; }
    }

}
