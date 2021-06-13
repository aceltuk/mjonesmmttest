using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MJonesMmtTest.Api.Models.Data;
using MJonesMmtTest.Api.Models.Dto;

namespace MJonesMmtTest.Api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SseTestContext _context;

        public OrderRepository(SseTestContext context)
        {
            _context = context;
        }

        public async Task<Order> GetLastOrder(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return null;

            var query = _context.Orders.Where(o => o.Customerid == customerId)
                .Include(oi => oi.Orderitems).ThenInclude(p => p.Product).OrderByDescending(o => o.Orderdate).Take(1);

            var result = await query.SingleOrDefaultAsync();
            return result;
        }
    }
}
