using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models
{
    [TableAttribute(name: "VwOrdDetailsProducts")]
    public class OrderDetails
    {
        //自動屬性
        [KeyAttribute]
        public Guid id { set; get; }
        public Int32 OrderID { set; get; }
        public Int32 ProductID { set; get; }
        public String ProductName { set; get; }
        public Decimal? UnitPrice { set; get; }
        public Int16? Quantity { set; get; }
        public Single? Discount { set; get; }
        public Single? amt { set; get; }
    }
}
