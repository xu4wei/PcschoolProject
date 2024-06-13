namespace MyWeb.Models
{
    //客戶Entity Class
    public class Customers
    {
        public String customerId {  get; set; }
        public String companyName { get; set; }
        public String? address { get; set; }
        public String? phone { get; set; }
        public String? country { get; set; }
        //建立日期
        public DateTime? createdDate { get; set; }
    }
}
