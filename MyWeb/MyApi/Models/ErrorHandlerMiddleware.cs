using System.Net;
using System.Text.Json.Serialization;

namespace MyApi.Models
{
    //POCO(plain Old CLR Object)
    public class ErrorHandlerMiddleware
    {
        //Data Field 參照一個注入進來的RequestDelegate物件(請求委派物件)
        private readonly RequestDelegate _next;
        //參照Logger物件
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        //建構子注入依賴物件
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger) 
        {
            Console.WriteLine("ErrorHandlerMiddleware 建構子被呼叫");
            _next = next;
            _logger = logger;
        }
        //Middleware核心方法 採用非同步處理
        //注入HttpContext物件 進行一個前端請求處理與回應物件
        public async Task Invoke(HttpContext context)
        {
            try
            {
                Console.WriteLine("ErrorHandlerMiddleware.Invoke()被呼叫");
                //呼叫下一個Middleware
                //不進行RequestDelegate物件的呼叫 則無法往下走Middleware Pipeline(卡在這裡 沒有進出)
                //forward 向前走 同時持續原來的Request/Response 到下個目標走(Middleware or Controlle-Action)
                await _next(context); //往下走 Middleware Pipeline
            }
            catch(Exception ex) 
            {
                //自訂錯誤處理
                await HandleExceptionAsync(context, ex);
            }
        }
        //自訂錯誤處理(採用private)
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //設定回應狀態碼
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //後端內部嚴重錯誤 500
            //設定回應內容
            context.Response.ContentType = "application/json";
            //建構一個錯誤訊息物件
            var error = new
            {
                Message = ex.Message,
                stackTrace = ex.StackTrace, //反應發生來源點
                code = context.Response.StatusCode
                //...其他屬性
            }; //建構匿名型別
            //寫入Log
            _logger.LogError($"Logging錯誤訊息:{ex.Message}");

            //序列化錯誤訊息物件
            String? result = Newtonsoft.Json.JsonConvert.SerializeObject(error);
            //寫入回應內容
            await context.Response.WriteAsync(result);
        }
    }
}
