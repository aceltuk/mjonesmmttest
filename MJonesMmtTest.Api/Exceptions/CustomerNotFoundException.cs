using System;

namespace MJonesMmtTest.Api.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string emailAddress)
            : base($"Could not find any customer information using email: {emailAddress}") { }
    }
}
