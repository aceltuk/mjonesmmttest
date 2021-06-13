using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MJonesMmtTest.Api.Models.Dto;
using MJonesMmtTest.Api.Repositories;
using MJonesMmtTest.Api.Services;
using Moq;
using Xunit;

namespace MJonesMmtTest.Api.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        [Trait("TestType", "Unit")]

        public async Task GetOrderDetails_CustomerIdEmpty_ReturnsNull()
        {
            //arrange
            var orderRepository = new Mock<IOrderRepository>();
            var orderService = new OrderService(orderRepository.Object);

            //act
            var orderServiceResult = await orderService.GetOrderDetails(string.Empty);

            //assert
            orderServiceResult.Should().BeNull();
            orderRepository.Verify(x => x.GetLastOrder(string.Empty), Times.Never);
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public async Task GetOrderDetails_CustomerIdNotFound_ReturnsNull()
        {
            //arrange
            var orderRepository = new Mock<IOrderRepository>();
            orderRepository.Setup(s => s.GetLastOrder("12345")).ReturnsAsync((Order)null);
            var orderService = new OrderService(orderRepository.Object);

            //act
            var orderServiceResult = await orderService.GetOrderDetails("12345");

            //assert
            orderServiceResult.Should().BeNull();
            orderRepository.Verify(x => x.GetLastOrder("12345"), Times.AtMostOnce);
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public async Task GetOrderDetails_CustomerFound_ReturnsOneOrderWithOneItem()
        {
            //arrange
            var orderRepository = new Mock<IOrderRepository>();
            orderRepository.Setup(s => s.GetLastOrder("12345")).ReturnsAsync(new Order
            {
                Orderid = 1,
                Orderitems = new List<Orderitem>
                {
                    new Orderitem
                    {
                        Product = new Product { Productid = 1 },
                        Orderitemid = 1,
                        Orderid = 1
                    }
                }
            });
            var orderService = new OrderService(orderRepository.Object);

            //act
            var orderServiceResult = await orderService.GetOrderDetails("12345");

            //assert
            orderServiceResult.OrderItems.Should().HaveCount(1);
            orderRepository.Verify(x => x.GetLastOrder("12345"), Times.AtMostOnce);
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public async Task GetOrderDetails_CustomerFound_ReturnsOneOrderWithTwoItems_NoneAreGiftsAndShouldUseTheirProductNames()
        {
            //arrange
            var orderRepository = new Mock<IOrderRepository>();
            orderRepository.Setup(s => s.GetLastOrder("12345")).ReturnsAsync(new Order
            {
                Orderid = 1,
                Containsgift = false,
                Orderitems = new List<Orderitem>
                {
                    new Orderitem
                    {
                        Product = new Product { Productid = 1, Productname = "Plane" },
                        Orderitemid = 1,
                        Orderid = 1
                    },
                    new Orderitem
                    {
                        Product = new Product { Productid = 2, Productname = "Train" },
                        Orderitemid = 2,
                        Orderid = 1
                    }
                }
            });
            var orderService = new OrderService(orderRepository.Object);

            //act
            var orderServiceResult = await orderService.GetOrderDetails("12345");
            var orderItems = orderServiceResult.OrderItems.ToList();
            var product1 = orderItems[0].Product;
            var product2 = orderItems[1].Product;

            //assert
            orderServiceResult.OrderItems.Should().HaveCount(2);
            orderRepository.Verify(x => x.GetLastOrder("12345"), Times.AtMostOnce);
            product1.Should().Be("Plane");
            product2.Should().Be("Train");
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public async Task GetOrderDetails_CustomerFound_ReturnsOneOrderWithTwoItems_BothAreGiftsAndShouldHaveTheirNamesReplacedWithTheWordGift()
        {
            //arrange
            var orderRepository = new Mock<IOrderRepository>();
            orderRepository.Setup(s => s.GetLastOrder("12345")).ReturnsAsync(new Order
            {
                Orderid = 1,
                Containsgift = true,
                Orderitems = new List<Orderitem>
                {
                    new Orderitem
                    {
                        Product = new Product { Productid = 1, Productname = "Plane" },
                        Orderitemid = 1,
                        Orderid = 1
                    },
                    new Orderitem
                    {
                        Product = new Product { Productid = 2, Productname = "Train" },
                        Orderitemid = 2,
                        Orderid = 1
                    }
                }
            });
            var orderService = new OrderService(orderRepository.Object);

            //act
            var orderServiceResult = await orderService.GetOrderDetails("12345");
            var orderItems = orderServiceResult.OrderItems.ToList();
            var product1 = orderItems[0].Product;
            var product2 = orderItems[1].Product;

            //assert
            orderServiceResult.OrderItems.Should().HaveCount(2);
            orderRepository.Verify(x => x.GetLastOrder("12345"), Times.AtMostOnce);
            product1.Should().Be("Gift");
            product2.Should().Be("Gift");
        }

        [Fact]
        [Trait("TestType", "Unit")]

        public async Task GetOrderDetails_CustomerFound_ReturnsOneOrderWithOneItem_CheckNullableValues()
        {
            //arrange
            var orderRepository = new Mock<IOrderRepository>();
            orderRepository.Setup(s => s.GetLastOrder("12345")).ReturnsAsync(new Order
            {
                Orderid = 1,
                Containsgift = true,
                Deliveryexpected = null,
                Orderdate = null,
                Orderitems = new List<Orderitem>
                {
                    new Orderitem
                    {
                        Product = new Product { Productid = 1, Productname = "Plane" },
                        Orderitemid = 1,
                        Orderid = 1,
                        Price = null,
                        Productid = null,
                        Quantity = null
                    }
                }
            });
            var orderService = new OrderService(orderRepository.Object);

            //act
            var orderServiceResult = await orderService.GetOrderDetails("12345");
            var orderItems = orderServiceResult.OrderItems.ToList();
            var product1 = orderItems[0];

            //assert
            orderServiceResult.OrderItems.Should().HaveCount(1);
            orderRepository.Verify(x => x.GetLastOrder("12345"), Times.AtMostOnce);
            orderServiceResult.DeliveryExpected.Should().BeNullOrWhiteSpace();
            orderServiceResult.OrderDate.Should().BeNullOrWhiteSpace();
            product1.PriceEach.Should().Be(0);
            product1.Quantity.Should().Be(0);
        }
    }
}
