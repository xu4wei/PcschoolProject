namespace MyWeb.Models
{
    //Entity class-採用POCO(Plain Old CLR Object)純粹的C#物件
    public class Member
    {
        //自動屬性(抽象屬性 編譯實作方法與Data Field)
        public String userName { set; get; }
        public String password { set; get; }

    }
}
