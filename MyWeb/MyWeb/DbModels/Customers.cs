using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWeb.DbModels
{
    //Entity class Mapping Table-Customers Fields
    //描述Table Name
    [TableAttribute(name:"Customers")]
    public class Customers
    {
        //識別用欄位(PK or UK)
        [KeyAttribute]
        [ColumnAttribute(name: "CustomerID")]
        [Required()] //非null
        public String CustomerID { get; set; }
        [ColumnAttribute(name: "CompanyName")]
        [Required()] //非null
        public String CompanyName { get; set; }
        [ColumnAttribute(name: "Address")]
        public String? Address { get; set; }
        [ColumnAttribute(name: "Phone")]
        public String? Phone { get; set; }
        [ColumnAttribute(name: "Country")]
        public String? Country { get; set; }
        

    }
}
