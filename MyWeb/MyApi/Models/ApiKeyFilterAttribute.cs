using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyApi.Models
{
    public class ApiKeyFilterAttribute:ActionFilterAttribute
    {
        //Overriding 覆寫 Action Before Cutting(AOP設計模式)
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            //取出Request 封裝是否帶有Header apikey???(進行安全驗證)
            if(!context.HttpContext.Request.Headers.ContainsKey("apikey"))
            {
                //沒有帶有apikey(不能進入Action執行 回應 ActionResult 自訂Result物件)
                Message msg = new Message()
                {
                    code = 401,
                    message = "沒有帶入apikey 不能通行!!"
                };
                context.Result = new UnauthorizedObjectResult(msg); //http status code 401
            }
            else
            {
                //取出apikey
                string apikey = context.HttpContext.Request.Headers["apikey"];
                //TODO 比對apikey是否正確(往往跟資料表驗證有關)
                //..驗證通過了
            }
        }
    }
}
