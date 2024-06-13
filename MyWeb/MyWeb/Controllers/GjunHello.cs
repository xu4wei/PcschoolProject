using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    //類別名稱沒有Follow XxxxController 命名
    [RouteAttribute("pcschool")]
    public class GjunHello : Controller
    {
        [RouteAttribute("helloworld/{who}/html")]
        public IActionResult Index(String who)
        {
            return Content($"{who} 您好 世界和平");
        }
        //打招呼 RouteAttribute可以多重設定
        [RouteAttribute("hello")]
        [RouteAttribute("hello/html")]
        public IActionResult helloWorld() 
        {
            //第二個參數回應是MIME Type(Content-Type)--回應Header
            return Content("<font size='36' color='green'>Hello World</font>","text/html");
        }
    }
}
