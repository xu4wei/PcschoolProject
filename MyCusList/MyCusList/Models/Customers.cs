namespace MyCusList.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Customers
    {
        [Key] // Primary Key Field
        [StringLength(5)]
        [ColumnAttribute(name: "CustomerID")]
        public string CustomerID { get; set; }

        [Required] //Required Field Validator
        [StringLength(40)]
        public string CompanyName { get; set; }

        [StringLength(30)]
        public string ContactName { get; set; }

        [StringLength(30)]
        public string ContactTitle { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        //? 結構型別NULL
        public DateTime? CreateDate { get; set; }

        //特殊型別名稱money
        [Column(TypeName = "money")]
        public decimal? Totalamt { get; set; }
    }
}
