using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;

namespace MyWeb.Controllers
{
    //MVC控制器 進行會員資料操作控制器
    public class MemberController : Controller
    {
        #region 註冊作業
        //提供註冊的頁面(Razor Page)
        public IActionResult register()
        {
            //todo 回應一個ViewResult物件 交給Razor引擎處理(樣板引擎 template engine--具有樣板資料夾)
            return View(); //沒有回應View Page名稱 預設會找Member/register.cshtml
        }

        //會員註冊儲存作業Action
        public IActionResult save(String userName,String password,String realName,String email) 
        {
            //todo 將會員資料儲存到資料庫
            return Content($"名稱:{userName} 密碼:{password} 真實姓名:{realName} EMAIL:{email}");
        }
        #endregion
        //查詢作業(同時也是可以調用表單頁面 進行查詢/傳遞一個使用者帳號與密碼 進行查詢)
        //介面多型化應用
        //注入 Injection 一個Member物件 在封裝前端傳遞近來欄位內容
        public IActionResult login(Member member) //依照類型 注入一個Member物件 
        {
            //如何透過傳送方法 判斷第一次請求 或者是postback 進行登入驗證
            //參照出封裝前端資訊的HttpRequest物件
            HttpRequest request = this.Request;
            //判斷是否是第一次請求 Request.Method == "GET"
            if(request.Method == "GET")
            {
                member.userName = "guest";
                //第一次調用查詢頁面(View Page)
                Console.WriteLine("第一次請求..." + member.userName);
                
            }
            else //if(request.Method == "POST")
            {
                //todo 登入驗證的結果 回應不同的View
                Console.WriteLine("登入驗證.." + member.userName);
                
            }
            //持續Member物件狀態到 View Page去(進行畫面HTML Render包含有Binding) 採用Model傳遞方式
            //指定View Page名稱
            return View("MyLogin");
        }

        //會員資料查詢作業(採用URL Path傳遞使用者名稱與密碼)
        //使用路由Attribute進行路由設定
        [RouteAttribute("/member/id/{name}/pwd/{password}/query")]
        public IActionResult query([FromRoute(Name ="name")]String userName,
            [FromRoute(Name ="password")]String pwd) 
        {
            //todo 進行會員資料查詢
            return Content($"會員名稱:{userName} 密碼:{pwd}");
        }
    }
}
