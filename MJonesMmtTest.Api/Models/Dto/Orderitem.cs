using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MJonesMmtTest.Api.Models.Dto
{
    public partial class Orderitem
    {
        public int Orderitemid { get; set; }

        public int? Orderid { get; set; }

        public int? Productid { get; set; }

        public int? Quantity { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal? Price { get; set; }

        public bool? Returnable { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
