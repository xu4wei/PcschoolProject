namespace MyWeb.Models
{
    public class ApiConfig
    {
        //自動屬性(屬性名稱對應到appsettings.json 客製化JSON物件屬性)
        public String apikey { get; set; }
        public String secpath { get; set; }
    }
}
