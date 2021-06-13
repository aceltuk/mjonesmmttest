using Microsoft.EntityFrameworkCore;
using MJonesMmtTest.Api.Models.Dto;

namespace MJonesMmtTest.Api.Models.Data
{
    public class SseTestContext : DbContext
    {
        public SseTestContext(DbContextOptions<SseTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Orderitem> Orderitems { get; set; }

        public virtual DbSet<Product> Products { get; set; }
    }
}
