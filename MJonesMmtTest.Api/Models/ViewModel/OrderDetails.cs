using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MJonesMmtTest.Api.Models.ViewModel
{
    public class OrderDetails
    {
        public OrderDetails()
        {
            OrderItems = new List<OrderItem>();
        }

        public long OrderNumber { get; set; }

        public string OrderDate { get; set; }

        public string DeliveryAddress { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }

        public string DeliveryExpected { get; set; }

        /// <summary>To be used for comparison purposes to detect a customer/order mismatch</summary>
        [IgnoreDataMember] public string CustomerId { get; set; }
    }
}
