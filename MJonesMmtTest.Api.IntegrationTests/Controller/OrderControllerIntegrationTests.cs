using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MJonesMmtTest.Api.Models.Queries;
using Newtonsoft.Json;
using Xunit;

namespace MJonesMmtTest.Api.IntegrationTests.Controller
{
    public class OrderControllerIntegrationTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public OrderControllerIntegrationTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        [Trait("TestType", "Integration")]
        public async Task IntegrationTest_RetrieveOrder_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            const string request = "/api/order/last";

            // Act
            var query = new LastOrderQuery { User = "", CustomerId = "" };
            var response = await _client.PostAsync(request, new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        [Trait("TestType", "Integration")]
        public async Task IntegrationTest_RetrieveOrder_CustomerNotFound_ReturnsNotFound()
        {
            // Arrange
            const string request = "/api/order/last";

            // Act
            var query = new LastOrderQuery { User = "a@a.com", CustomerId = "R12345" };
            var response = await _client.PostAsync(request, new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        [Trait("TestType", "Integration")]
        public async Task IntegrationTest_RetrieveOrder_CustomerAndOrderMismatched_ReturnsConflict()
        {
            // Arrange
            const string request = "/api/order/last";

            // Act
            var query = new LastOrderQuery { User = "cat.owner@mmtdigital.co.uk", CustomerId = "C34455" };
            var response = await _client.PostAsync(request, new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(409);
        }

        [Fact]
        [Trait("TestType", "Integration")]
        public async Task IntegrationTest_RetrieveOrder_CustomerCorrect_ReturnsValidResult()
        {
            // Arrange
            const string request = "/api/order/last";

            // Act
            var query = new LastOrderQuery { User = "cat.owner@mmtdigital.co.uk", CustomerId = "C34454" };
            var response = await _client.PostAsync(request, new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(200);
        }
    }
}
