using System;

namespace MJonesMmtTest.Api.Exceptions
{
    public class CustomerAndOrderNotMatchedException : Exception
    {
        public CustomerAndOrderNotMatchedException(string suppliedEmailAddress, string suppliedCustomerId, string retrievedCustomerId)
            : base($"The customer record retrieved (customerId: {retrievedCustomerId}) does not match the supplied details (suppliedCustomerId: {suppliedCustomerId}, suppliedEmailAddress: {suppliedEmailAddress})") { }
    }
}
