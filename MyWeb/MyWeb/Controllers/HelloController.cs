using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    public class HelloController : Controller
    {
        [RouteAttribute("gjun/hello")]//自訂路由配置 穿插到Middleware之前
        public IActionResult helloWorld()
        {
            return Content("<font size='6' color='red'>Hello world</font>","text/html");
        }
    }
}
