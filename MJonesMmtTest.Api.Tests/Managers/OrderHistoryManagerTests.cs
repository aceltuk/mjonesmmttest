using System;
using System.Threading.Tasks;
using FluentAssertions;
using MJonesMmtTest.Api.Exceptions;
using MJonesMmtTest.Api.Managers;
using MJonesMmtTest.Api.Models.Dto;
using MJonesMmtTest.Api.Models.ViewModel;
using MJonesMmtTest.Api.Services;
using Moq;
using Xunit;

namespace MJonesMmtTest.Api.Tests.Managers
{
    public class OrderHistoryManagerTests
    {
        [Fact]
        [Trait("TestType", "Unit")]
        public void GetLastOrder_InvalidRequest_ReturnsRequestValidationException()
        {
            //arrange
            var customerService = new Mock<ICustomerService>();
            var orderService = new Mock<IOrderService>();
            var orderHistoryManager = new OrderHistoryManager(customerService.Object, orderService.Object);

            //act
            Func<Task> f = async () => { await orderHistoryManager.GetLastOrder(string.Empty, string.Empty); };

            //assert
            f.Should().Throw<RequestValidationException>();
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public void GetLastOrder_InvalidEmail_ReturnsCustomerNotFoundException()
        {
            //arrange
            var customerService = new Mock<ICustomerService>();
            var orderService = new Mock<IOrderService>();
            customerService.Setup(s => s.GetCustomerAsync("a@a.com")).ReturnsAsync((ApiCustomer)null);
            var orderHistoryManager = new OrderHistoryManager(customerService.Object, orderService.Object);

            //act
            Func<Task> f = async () => { await orderHistoryManager.GetLastOrder("a@a.com", "12345"); };

            //assert
            f.Should().Throw<CustomerNotFoundException>();
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public void GetLastOrder_MismatchedCustomerIds_ReturnsCustomerAndOrderNotMatchedException()
        {
            //arrange
            var customerService = new Mock<ICustomerService>();
            var orderService = new Mock<IOrderService>();
            customerService.Setup(s => s.GetCustomerAsync("a@a.com")).ReturnsAsync(new ApiCustomer { CustomerId = "12345" });
            orderService.Setup(s => s.GetOrderDetails("12345")).ReturnsAsync(new OrderDetails { CustomerId = "67896" });
            var orderHistoryManager = new OrderHistoryManager(customerService.Object, orderService.Object);

            //act
            Func<Task> f = async () =>
            {
                await orderHistoryManager.GetLastOrder("a@a.com", "12345");
            };

            //assert
            f.Should().Throw<CustomerAndOrderNotMatchedException>();
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public async Task GetLastOrder_CustomerFoundWithNoOrders_ReturnsOrderHistoryWithCustomerAndEmptyOrdersNode()
        {
            //arrange
            var customerService = new Mock<ICustomerService>();
            var orderService = new Mock<IOrderService>();
            customerService.Setup(s => s.GetCustomerAsync("a@a.com")).ReturnsAsync(new ApiCustomer { CustomerId = "12345", FirstName = "Charles", LastName = "Smith" });
            orderService.Setup(s => s.GetOrderDetails("12345")).ReturnsAsync((OrderDetails)null);
            var orderHistoryManager = new OrderHistoryManager(customerService.Object, orderService.Object);

            //act
            var orderHistoryResult = await orderHistoryManager.GetLastOrder("a@a.com", "12345");

            //assert
            orderHistoryResult.Customer.Firstname.Should().Be("Charles");
            orderHistoryResult.Customer.Lastname.Should().Be("Smith");
            orderHistoryResult.Order.Should().BeNull();
        }
    }
}
