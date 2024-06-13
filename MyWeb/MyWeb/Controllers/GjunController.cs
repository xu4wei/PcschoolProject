using Microsoft.AspNetCore.Mvc;
//Gjun 控制器
namespace MyWeb.Controllers
{
    public class GjunController : Controller
    {
        public IActionResult news()
        {
            //回應一個文字標籤
            return Content("您好 巨匠電腦");
        }
    }
}
