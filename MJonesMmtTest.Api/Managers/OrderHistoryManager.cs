using System;
using System.Threading.Tasks;
using MJonesMmtTest.Api.Exceptions;
using MJonesMmtTest.Api.Models.ViewModel;
using MJonesMmtTest.Api.Services;

namespace MJonesMmtTest.Api.Managers
{
    public class OrderHistoryManager : IOrderHistoryManager
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public OrderHistoryManager(ICustomerService customerService, IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        public async Task<OrderHistory> GetLastOrder(string emailAddress, string customerId)
        {
            //email and customerid are both required
            if (string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(customerId))
                throw new RequestValidationException(emailAddress, customerId);

            //not found in the MMT Test API
            var customer = await _customerService.GetCustomerAsync(emailAddress);
            if (customer == null)
                throw new CustomerNotFoundException(emailAddress);

            //customer found, now lets store those details in the return object and get any order information
            var orderHistory = new OrderHistory(customer.FirstName, customer.LastName);
            var orderDetails = await _orderService.GetOrderDetails(customer.CustomerId);

            //no order details found - ok, that is valid, return the OrderHistory object as currently populated
            if (orderDetails == null)
                return orderHistory;

            //order details found, check that the customer ids match - if not, flag
            if (!orderDetails.CustomerId.Equals(customerId, StringComparison.OrdinalIgnoreCase))
                throw new CustomerAndOrderNotMatchedException(emailAddress, customerId, orderDetails.CustomerId);

            //attach any order details to the history object and return
            orderHistory.Order = orderDetails;
            return orderHistory;
        }
    }
}
