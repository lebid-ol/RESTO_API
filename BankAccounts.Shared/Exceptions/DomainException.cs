using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankAccounts.Shared.Exceptions
{
    public class DomainException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? ErrorCode { get; }
        public string? Details { get; }

        public DomainException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            string? errorCode = null,
            string? details = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Details = details;
        }
    }
}
    
