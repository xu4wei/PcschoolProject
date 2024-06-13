using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;

namespace MyWeb.Controllers
{
    public class CustomerClientController : Controller
    {
        //Data Field
        //管理一個注入進來的HttpClientFactory物件
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MyWeb.Models.CustomersApi _customersApi;
        //自訂建構子 建構子注入
        public CustomerClientController(IHttpClientFactory httpClientFactory, Models.CustomersApi customersApi)
        {
            _httpClientFactory = httpClientFactory;
            _customersApi = customersApi;
        }
        //參數注入Customers Entity物件
        public IActionResult customersAdd(MyWeb.DbModels.Customers customers)
        {
            //判斷是第一次請求 或者Postback(POST)進行呼喚web api服務進行新增作業
            if (this.Request.Method.Equals("POST"))
            {
                //呼喚服務進行新增作業 HttpClient(預設被關閉 從program.cs加入服務)
                //1. 將物件序列化成Json String
                String jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(customers);
                Console.WriteLine(jsonString);
                //2. 跟工廠要一個 HttpClient物件
                HttpClient client = _httpClientFactory.CreateClient();
                //3.設定HttpClient的基本屬性 BsaeAddress Uri物件(服務端位址封裝)
                client.BaseAddress = new Uri(_customersApi.addurl);
                //Content-Type:application/json 屬性Http Body設定內配合Json格式
                //建構一個HttpContent物件
                HttpContent content = new StringContent(jsonString, System.Text.Encoding.UTF8);
                //設定Content-Type:application/json
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //4.呼叫PostAsync方法進行新增作業
                HttpResponseMessage response = client.PostAsync("", content).GetAwaiter().GetResult();
                //5.取得回應結果
                if (response.IsSuccessStatusCode)
                {
                    //取得回應內容
                    String? result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //將回應內容反序列化成物件
                    Models.Message? msg = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Message>(result);
                    ViewData["msg"] = msg.message;
                    //將物件傳遞到View
                }
                //回應狀態碼是 400
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //400
                {
                    //取得錯誤訊息
                    String result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //將錯誤訊息傳遞到View
                    //將回應內容反序列化成物件
                    Message? msg = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(result);
                    ViewData["msg"] = msg.message;

                }

            }
            return View();
        }

        //提供一個前端頁面 進行Client side ajax  invoke RESTful service
        public IActionResult customersall() 
        {
            //調用頁面
            String geturl = _customersApi.geturl;
            //帶入狀態到View Page 進行JS Global變數 Embeding
            ViewData["geturl"] = geturl;
            return View();
        }
    }
}
