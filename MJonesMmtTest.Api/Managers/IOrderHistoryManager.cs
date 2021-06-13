using System.Threading.Tasks;
using MJonesMmtTest.Api.Models.ViewModel;

namespace MJonesMmtTest.Api.Managers
{
    public interface IOrderHistoryManager
    {
        public Task<OrderHistory> GetLastOrder(string emailAddress, string customerId);
    }
}
