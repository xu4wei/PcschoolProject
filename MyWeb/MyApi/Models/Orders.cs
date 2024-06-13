using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models
{
    /// <summary>
    /// 訂單主檔與明細檔關聯資訊
    /// </summary>
    [TableAttribute(name: "VWCustomersOrders")]
    public class Orders
    {
        //自動屬性
        /// <summary>
        /// 訂單編號_整數
        /// </summary>
        [KeyAttribute]
        public Int32 OrderID { set; get; }
        /// <summary>
        /// 客戶編號
        /// </summary>
        public String CustomerID { set; get; }
        /// <summary>
        /// 公司行號
        /// </summary>
        public String CompanyName { set; get; }
        /// <summary>
        /// 聯絡地址
        /// </summary>
        public String Address { set; get; }
        /// <summary>
        /// 國家別
        /// </summary>
        public String Country { set; get; }
        /// <summary>
        /// 連絡電話
        /// </summary>
        public String Phone { set; get; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        public DateTime? OrderDate { set; get; }
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequiredDate { set; get; }
        /// <summary>
        /// 出貨日期
        /// </summary>
        public DateTime? ShippedDate { set; get; }

    }
}
