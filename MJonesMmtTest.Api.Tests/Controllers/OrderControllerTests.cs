using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MJonesMmtTest.Api.Controllers;
using MJonesMmtTest.Api.Exceptions;
using MJonesMmtTest.Api.Managers;
using MJonesMmtTest.Api.Models.Queries;
using MJonesMmtTest.Api.Models.ViewModel;
using Moq;
using Xunit;

namespace MJonesMmtTest.Api.Tests.Controllers
{
    public class OrderControllerTests
    {
        [Fact]
        [Trait("TestType", "Unit")]
        public async Task RetrieveOrder_InvalidRequest_ReturnsBadRequest()
        {
            //arrange
            var orderHistoryManager = new Mock<IOrderHistoryManager>();
            var logger = new Mock<ILogger<OrderController>>();
            orderHistoryManager.Setup(s => s.GetLastOrder(It.IsAny<string>(), It.IsAny<string>())).Throws(new RequestValidationException("", ""));
            var orderController = new OrderController(orderHistoryManager.Object, logger.Object);
            var query = new LastOrderQuery { CustomerId = "", User = "" };

            //act
            var actionResult = await orderController.RetrieveLastOrder(query);

            //assert
            var result = actionResult.Result as BadRequestObjectResult;
            actionResult.Value.Should().Be(null, "");
            result.Should().NotBeNull();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public async Task RetrieveOrder_CustomerNotFound_ReturnsNotFound()
        {
            //arrange
            var orderHistoryManager = new Mock<IOrderHistoryManager>();
            var logger = new Mock<ILogger<OrderController>>();
            orderHistoryManager.Setup(s => s.GetLastOrder(It.IsAny<string>(), It.IsAny<string>())).Throws(new CustomerNotFoundException(""));
            var orderController = new OrderController(orderHistoryManager.Object, logger.Object);
            var query = new LastOrderQuery { CustomerId = "", User = "" };

            //act
            var actionResult = await orderController.RetrieveLastOrder(query);

            //assert
            var result = actionResult.Result as NotFoundObjectResult;
            actionResult.Value.Should().Be(null, "");
            result.Should().NotBeNull();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public async Task RetrieveOrder_CustomerAndOrderMismatched_ReturnsConflict()
        {
            //arrange
            var orderHistoryManager = new Mock<IOrderHistoryManager>();
            var logger = new Mock<ILogger<OrderController>>();
            orderHistoryManager.Setup(s => s.GetLastOrder(It.IsAny<string>(), It.IsAny<string>())).Throws(new CustomerAndOrderNotMatchedException("", "", ""));
            var orderController = new OrderController(orderHistoryManager.Object, logger.Object);
            var query = new LastOrderQuery { CustomerId = "", User = "" };

            //act
            var actionResult = await orderController.RetrieveLastOrder(query);

            //assert
            var result = actionResult.Result as ConflictObjectResult;
            actionResult.Value.Should().Be(null, "");
            result.Should().NotBeNull();
        }

        [Fact]
        [Trait("TestType", "Unit")]
        public async Task RetrieveOrder_CustomerCorrect_ReturnsValidResult()
        {
            //arrange
            var orderHistoryManager = new Mock<IOrderHistoryManager>();
            var logger = new Mock<ILogger<OrderController>>();
            var orderHistoryResult = new OrderHistory("Steve", "Smith");

            orderHistoryManager.Setup(s => s.GetLastOrder(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(orderHistoryResult);
            var orderController = new OrderController(orderHistoryManager.Object, logger.Object);
            var query = new LastOrderQuery { CustomerId = "", User = "" };

            //act
            var actionResult = await orderController.RetrieveLastOrder(query);

            //assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
        }
    }
}
