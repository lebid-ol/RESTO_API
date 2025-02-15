public class Program
{
    public static void Main()
    {
        var (user, failure) = GetUser();




        if (user == null) 
        {
            return failure.Description;
        }
    }

    public static (User, Failure) GetUser()
    {
        // if user not exist retuirn faulure

        return (null, new Failure() { Description = "User not found"});

        return (new User(),  null);
    }

}