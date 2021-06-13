using System.Threading.Tasks;
using MJonesMmtTest.Api.Models.ViewModel;

namespace MJonesMmtTest.Api.Services
{
    public interface IOrderService
    {
        public Task<OrderDetails> GetOrderDetails(string customerId);
    }
}