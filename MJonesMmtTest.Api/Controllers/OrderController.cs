using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MJonesMmtTest.Api.Exceptions;
using MJonesMmtTest.Api.Managers;
using MJonesMmtTest.Api.Models.Queries;
using MJonesMmtTest.Api.Models.ViewModel;
using Newtonsoft.Json;

namespace MJonesMmtTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderHistoryManager _orderHistoryManager;
        private readonly ILogger _logger;

        public OrderController(IOrderHistoryManager orderHistoryManager, ILogger logger)
        {
            _orderHistoryManager = orderHistoryManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<OrderHistory>> RetrieveLastOrder([FromBody] LastOrderQuery query)
        {
            OrderHistory orderHistory;

            try
            {
                _logger.LogInformation($"Looking up order information for '{query.User}', '{query.CustomerId}'");
                orderHistory = await _orderHistoryManager.GetLastOrder(query.User, query.CustomerId);
            }
            catch (RequestValidationException valEx)
            {
                _logger.LogInformation(valEx.Message);
                return BadRequest("Invalid request received - please check your message.");
            }
            catch (CustomerNotFoundException notEx)
            {
                _logger.LogError(notEx.Message);
                return NotFound("Customer details not found - please check your message.");
            }
            catch (CustomerAndOrderNotMatchedException invEx)
            {
                _logger.LogError(invEx.Message);
                return Conflict("Customer details not found - please contact customer services.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An unexpected error has occurred.");
                throw;
            }

            _logger.LogInformation($"Returning: {JsonConvert.SerializeObject(orderHistory)}");
            return Ok(orderHistory);
        }
    }
}
