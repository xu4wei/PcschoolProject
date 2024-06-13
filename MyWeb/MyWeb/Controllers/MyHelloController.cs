using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    public class MyHelloController : Controller
    {
        //回應類型採用介面多型化
        public IActionResult hello()
        {
            //回應一個文字標籤 第二個參數 是決定Content-Type
            //Content Method主要用來回應文字內容到前端去 View
            return Content("<font size='6' color='red'>Hello WorlD</font>","text/html");
        }
        //調用一個Razor Page頁面 method as Action
        public ViewResult helloRazor() 
        {
            //調用(Dispathcher)一個Razor Page頁面 透過Razor Page引擎進行解析
            return View();//回應一個ViewResult物件 沒有客製化View Name 網頁採用同方法名稱一樣
        }
    }
}
