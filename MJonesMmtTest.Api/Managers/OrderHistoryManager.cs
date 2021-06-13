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
            if (string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(customerId))
                throw new RequestValidationException(emailAddress, customerId);

            var customer = await _customerService.GetCustomerAsync(emailAddress);
            if (customer == null)
                throw new CustomerNotFoundException(emailAddress);

            var orderHistory = new OrderHistory(customer.FirstName, customer.LastName);
            var orderDetails = await _orderService.GetOrderDetails(customer.CustomerId);

            if (orderDetails == null)
                return orderHistory;

            if (!orderDetails.CustomerId.Equals(customerId, StringComparison.OrdinalIgnoreCase))
                throw new CustomerAndOrderNotMatchedException(emailAddress, customerId, orderDetails.CustomerId);

            return orderHistory;
        }
    }
}
