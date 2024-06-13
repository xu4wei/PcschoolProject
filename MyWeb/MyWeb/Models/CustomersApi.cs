namespace MyWeb.Models
{
    //配合綁定appsettings.json customersApi區段
    public class CustomersApi
    {
        //自動屬性
        public String addurl { set; get; }
        public String updateurl { set; get; }
        public String deleteurl { set; get; }
        public String geturl { set; get; }
    }
}
