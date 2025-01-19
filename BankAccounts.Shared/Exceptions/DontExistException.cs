using System.Net;

namespace BankAccounts.Exceptions
{
    public class DontExistException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NoContent;
        public string Message { get; set; }

        public DontExistException(string message)
        {
            Message = message;
        }
    }
}
