using System;
using System.Threading.Tasks;
using FluentAssertions;
using MJonesMmtTest.Api.Exceptions;
using MJonesMmtTest.Api.Managers;
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
    }
}
