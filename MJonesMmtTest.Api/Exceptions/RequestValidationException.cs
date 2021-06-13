using System;

namespace MJonesMmtTest.Api.Exceptions
{
    public class RequestValidationException : Exception
    {
        public RequestValidationException(string emailAddress, string customerId) : base($"Invalid parameters received - email address: {emailAddress}, customerId: {customerId}")
        {

        }
    }
}
