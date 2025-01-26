namespace BankAccounts.API.DI_test
{
    public class TransientTest
    {
        public Guid Id { get; set; }

        public TransientTest()
        {
            Id = Guid.NewGuid();
        }
        public void ShowId()
        {
            Console.WriteLine("TRANSIENT SERVICE: " + Id.ToString());
        }
    }
}
