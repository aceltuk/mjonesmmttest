using System.Threading.Tasks;
using MJonesMmtTest.Api.Exceptions;
using MJonesMmtTest.Api.Models.ViewModel;
using MJonesMmtTest.Api.Services;

namespace MJonesMmtTest.Api.Managers
{
    public class OrderHistoryManager : IOrderHistoryManager
    {
        private ICustomerService _customerService;
        private IOrderService _orderService;

        public OrderHistoryManager(ICustomerService customerService, IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        public Task<OrderHistory> GetLastOrder(string emailAddress, string customerId)
        {
            if (string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(customerId))
                throw new RequestValidationException(emailAddress, customerId);

            return Task.FromResult(new OrderHistory("", ""));
        }
    }
}
