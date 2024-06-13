using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyWeb.Models
{
    public class ApiKeyFilter : IAsyncAuthorizationFilter
    {
        //Data Field
        private IWebHostEnvironment _env; //網站系統環境物件
        //apiconfig物件
        private ApiConfig _apiConfig;
        //建構子
        public ApiKeyFilter(IWebHostEnvironment _env, ApiConfig apiConfig)
        {
            this._env = _env;
            _apiConfig = apiConfig;
            Console.WriteLine("ApiKeyFilter建構子被呼叫..." + _apiConfig.secpath);
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //非同步非阻斷的處理 攔截到 Http Request Header中的ApiKey
            //1.取得封裝前端所有資訊的 Reqest物件(Proxy代理一個相對的前端)
            //取出前端請求的端點
            String webPath = _env.WebRootPath;
            Console.WriteLine("ContentRootPath:" + webPath); //wwwroot採用Real Path

            PathString path = context.HttpContext.Request.Path;
            Console.WriteLine("path:" + path.Value); //前端請求點
            String endpintVPath = path.Value;

            //Match 注入進來的需要稽核的paths
            //-------------------取出安全性目錄-------------------
            String[] secpaths = _apiConfig.secpath.Split(';');
            //透過陣列處理 判斷是否有包含請求相對端點
            if (secpaths.Contains(endpintVPath))
            {
                //要進行apikey驗證
                HttpContext currentContext = context.HttpContext;
                HttpRequest request = currentContext.Request;
                //2.取得Http Request Header中的ApiKey(key->value)
                IHeaderDictionary headers = request.Headers;
                //使用名稱取值
                String value = headers["apikey"];

                //沒有帶入apikey--非法(卡住 不往目標端點請求)
                if(String.IsNullOrEmpty(value)) 
                {
                    //回應一個錯誤訊息
                    Message message = new Message()
                    {
                        code = 401,
                        status = "ApiKey 沒有帶入"
                    };
                    //取得相對前端的HttpRespinse換掉回應Http status code
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(message);
                    //處理完畢
                    return Task.CompletedTask; //From Result--請求端點 往下走
                }
                else
                {
                    //3.判斷Apikey是否存在(合法)--資料庫資料表apikey驗證
                    if (!value.Equals(_apiConfig.apikey))
                    {
                        Console.WriteLine("Apikey:" + value);
                        //回應一個錯誤訊息
                        Message message = new Message()
                        {
                            code = 401,
                            status = "ApiKey驗證不合法!!!"
                        };
                        //取得相對前端的HttpRespinse換掉回應Http status code
                        context.HttpContext.Response.StatusCode = 401;
                        context.Result = new JsonResult(message);
                        //處理完畢
                        return Task.CompletedTask; //Form Result--請求端點 往下走
                    }
                    //驗證處理好了
                    return Task.CompletedTask; //Form Result--請求端點 往下走

                }
            }
            //無須驗證
            return Task.CompletedTask; //From Result--請求端點 往下走
        }
    }
}
