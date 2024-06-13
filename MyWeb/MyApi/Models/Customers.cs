using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models
{
    //Entity class for Customers
    [TableAttribute(name:"Customers")]
    public class Customers
    {
        //自動屬性
        [ColumnAttribute(name:"CustomerID")]
        [KeyAttribute]
        [RequiredAttribute]
        [MaxLength(5)]
        public String customerId {  get; set; }
        [ColumnAttribute(name: "CompanyName")]
        [RequiredAttribute]
        public String companyName { get; set; }
        [ColumnAttribute(name: "Address")]
        public String? address { get; set; }
        [ColumnAttribute(name: "Phone")]
        public String? phone { get; set; }
        [ColumnAttribute(name: "Country")]
        public String? country { get; set; }

    }
}
