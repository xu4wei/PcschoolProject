using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models
{
    //Mapper Table-訂單明細檔Table
    [TableAttribute("Order Details")]
    public class OrderDetailsTB
    {
        [KeyAttribute(),ColumnAttribute(Order = 0)]
        public int OrderID {  get; set; }
        [KeyAttribute(), ColumnAttribute(Order = 1)]
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public Int16 Quantity { get; set; }
        public Single Discount { get; set; }
    }
}
