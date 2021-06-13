using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MJonesMmtTest.Api.Models.Dto
{
    public partial class Product
    {
        public Product()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        public int Productid { get; set; }

        public string Productname { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal? Packheight { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal? Packwidth { get; set; }

        [Column(TypeName = "decimal(8, 3)")]
        public decimal? Packweight { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }

        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
