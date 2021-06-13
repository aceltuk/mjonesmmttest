using System.Linq;
using System.Threading.Tasks;
using MJonesMmtTest.Api.Models.ViewModel;
using MJonesMmtTest.Api.Repositories;

namespace MJonesMmtTest.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDetails> GetOrderDetails(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return null;

            var result = await _orderRepository.GetLastOrder(customerId);
            if (result == null)
                return null;

            var details = new OrderDetails
            {
                DeliveryExpected = result.Deliveryexpected.HasValue ? result.Deliveryexpected.Value.ToString("dd-MMM-yyyy") : string.Empty,
                OrderDate = result.Orderdate.HasValue ? result.Orderdate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                OrderNumber = result.Orderid,
                CustomerId = result.Customerid
            };

            const string isGift = "Gift"; //orders marked as containing gifts have their product names replaced with "Gift"
            var orderItems = result.Orderitems.Select(item => 
                new OrderItem
                {
                    PriceEach = item.Price.GetValueOrDefault(0), 
                    Product = result.Containsgift.GetValueOrDefault(false) ? isGift : item.Product.Productname, 
                    Quantity = item.Quantity.GetValueOrDefault(0)
                }).ToList();

            details.OrderItems = orderItems;

            return details;
        }
    }
}
