using System.Net;

namespace BankAccounts.Exceptions
{
    public class NotFoundException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;
        public string Message { get; set; }

        public NotFoundException(string message) 
        { 
            Message = message;
        }
    }
}
