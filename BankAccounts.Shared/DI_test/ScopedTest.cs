namespace BankAccounts.API.DI_test
{
    public class ScopedTest
    {
        public Guid Id {  get; set; }

       public ScopedTest()
        {
            Id = Guid.NewGuid();
        }

        public void ShowId()
        {
            Console.WriteLine("SCOPED SERVICE: " + Id.ToString());
        }
    }
}
