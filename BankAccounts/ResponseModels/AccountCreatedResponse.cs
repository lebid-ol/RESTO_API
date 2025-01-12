using BankAccounts.RequestModel;

namespace BankAccounts.ResponseModels
{
    public class AccountCreatedResponse
    {
        public Guid AccountID { get; set; }
        public int StartingBalance { get; set; }

        public bool IsCreated { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }

    }
}
