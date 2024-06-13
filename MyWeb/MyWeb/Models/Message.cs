namespace MyWeb.Models
{
    //回應Json文件格式內容的依據
    public class Message
    {
        //自動屬性
        public Int32 code { set; get; }
        public String status { set; get; }
        public String message { set; get; }
    }
}
