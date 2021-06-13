using System.Threading.Tasks;
using MJonesMmtTest.Api.Models.Dto;

namespace MJonesMmtTest.Api.Repositories
{
    public interface IOrderRepository
    {
        public Task<Order> GetLastOrder(string customerId);
    }
}
