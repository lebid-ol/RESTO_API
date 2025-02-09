namespace BankAccounts.API.DI_test
{
    public class SingletonTest
    {
        public Guid Id { get; set; }

        public SingletonTest()
        {
            Id = Guid.NewGuid();
        }
        public void ShowId()
        {
            Console.WriteLine("SINGLETON SERVICE: " + Id.ToString());
        }
    }
}
