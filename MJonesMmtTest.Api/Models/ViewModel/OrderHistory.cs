namespace MJonesMmtTest.Api.Models.ViewModel
{
    public class OrderHistory
    {
        public OrderHistory(string firstName, string lastName)
        {
            Customer = new Customer { Firstname = firstName, Lastname = lastName };
            Order = new OrderDetails();
        }

        public Customer Customer { get; set; }

        public OrderDetails Order { get; set; }
    }
}
