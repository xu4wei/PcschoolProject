using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MyWeb.DbModels;
using MyWeb.Models;
//客戶資料維護控制器
namespace MyWeb.Controllers
{
    public class CustomersController : Controller
    {
        //Constructor Injection 注入依賴物件DI(Dependency Injection)透過建構子注入
        private IDao<DbModels.Customers, String> _dao;
        //參照注入進來的DbContext物件
        private NorthwndContext _context;
        public CustomersController(IDao<DbModels.Customers, String> dao, NorthwndContext context)
        {
            _dao = dao;
            Console.WriteLine("CustomersController建構子被呼叫..." + _dao);
            _context = context;
        }

        //注入客戶物件(Customers entity class)
        public IActionResult customersAdd(DbModels.Customers customers)
        {
            //判斷是第一次請求 或者表單欄位postback
            //透過控制器Request Property參照出封裝一個前端資訊 Request物件
            HttpRequest request = this.Request;
            if (request.Method == "POST")
            {
                //TODO 進行客戶資料新增
            }
            else
            {
                //TODO 持續這一個客戶物件到Razor View去進行畫面HTML Render
                customers.CompanyName = "Microsoft";
            }
            return View();
        } //參數是區域變數 活在方法程序內

        //客戶資料查詢 傳遞國家別 From Field
        //String 注入一個字串物件
        public IActionResult customersQryByCountry(String country) 
        {
            List<DbModels.Customers> result = null;
            //判斷採用Request Method-GET(超連結或者輸入網址)or POST(一般配合表單頁面<form>標籤)
            HttpRequest request = this.Request;
            if(request.Method.Equals("GET"))
            {
                //超連結來的 直接調用表單頁面
                //回到原先表單頁面
                return View(result);
            }
            else 
            {
                //進行客戶查詢
                //使用注入DbContext配合LINQ查詢(延遲查詢)
                result=(from c in _context.Customers
                            where c.Country == country
                            select c).ToList();
                //將查詢結果集合物件傳遞到View Page
                ViewBag.country = country; //ViewBag 動態物件
                return View(result); //Model傳遞

            }
            
        }

        //配合Form Form Feild
        public IActionResult customersQryByCountry2(String country)
        {
            //先有一份資料 國家別清單(集合物件) List<SelectListItem>
            //1.建構集合物件List<SelectListItem>
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem("英國","UK"),
                new SelectListItem("美國","USA"),
                new SelectListItem("台灣","TW"),
                new SelectListItem("法國","France"),
            };//Object Initializer 物件初始化器

            //判斷採用Request Method-GET(超連結或者輸入網址)or POST(一般配合表單頁面<form>標籤)
            HttpRequest request = this.Request;
            if (request.Method.Equals("GET"))
            {
                //超連結來的 直接調用表單頁面
            }
            else
            {
                //todo 進行客戶查詢(先提供查詢的表單)
            }
            //使用ViewBag 屬性(dynamic)持續這一個建立好集合物件到 Razor Page進行渲染
            this.ViewBag.countryList = items;
            //回到原先表單頁面
            return View();
        }

        //採用QueryString 傳遞國家別 進行相關客戶資料查詢
        public IActionResult cunstomersQryByCountryParam([FromQueryAttribute(Name ="id")]String country) 
        {
            //todo 進行客戶資料查詢
            return Content($"國家別:{country}");
        }

        //採用route path as Parameter 傳遞國家別 進行相關客戶資料查詢
        [Route("/customers/qry/{id}/html")]
        public IActionResult customersQryByPath([FromRoute(Name ="id")]String country)
        {
            return Content($"國家別:{country}");
        }

        //查詢客戶所有資料
        public IActionResult customersQryAll()
        {
            List<DbModels.Customers> list = null;
            //提供互動UI(View)問傳送方法(如果採用GET or Post)
            if(this.Request.Method.Equals("GET"))
            {
                //第一次請求
            }
            else
            {
                //todo進行客戶資料查詢
                //使用注入進來dao物件進行查詢
                list = _dao.queryAll(); //呼叫dao自訂的方法
                //序列化集合物件為JSON(JavaScript Object Notation)String--使用JS Object and Array
                //借助第三方api(NuGet) Newtonsoft.json.dll
                String jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                //持續這一個jsonString到View Page Razor View進行崁入成JavaScript Array
                //使用ViewData Dictionary Property
                ViewDataDictionary dict = this.ViewData;
                //設定名稱(key)對應一個value 使用indexer語法
                dict["jsonString"] = jsonString;
            }
            //調用頁面(傳遞Model物件)
            return View(list); //將資料集合物件傳遞到View Page(採用強型別定義 變成網頁物件Model屬性參照)
        }

        //更新特定客戶資料 前端傳遞進來Json String資料 進行反序列化-更新資料表相對記錄
        //規範 Request Method-PUT (當作一個RESTful Service API) 同時客製化端點
        [HttpPutAttribute]
        [RouteAttribute("/api/customers/update/rawdata")]
        [ConsumesAttribute("application/json")] //要求前端採用Http Header Content-Type:application/json (MIME Type)
        public IActionResult customersUpdate([FromBodyAttribute] DbModels.Customers customers)
        {
            //資料更新作業(採用OR/M EntityFramework Core)--新增Entity到Persistence Context(DbContext)
            this._context.Customers.Add(customers); //新增一筆記錄物件狀態

            //改成更新狀態 同步回資料庫 才會產生Update SQL
            this._context.Entry(customers).State = EntityState.Modified;
            //檢視Entity狀態
            EntityState state = this._context.Entry(customers).State;

            Console.WriteLine("Entity State:" + state);
            Message msg = new Message();
            //同步更新回資料庫
            try
            {
                Int32 counter = this._context.SaveChanges();
                if (counter == 0)
                {
                    //更新不到原來資料
                    msg.code = 400; //bad request
                    msg.status = "更新失敗";
                    this.Response.StatusCode = 400;
                }
                else
                {
                    //更新成功
                    msg.code = 200; //ok
                    msg.status = "更新成功";
                    this.Response.StatusCode = 200;
                }
            }
            catch (DbUpdateException ex)
            {
                //更新失敗
                msg.code = 400; //Internal Server Error
                msg.status = "更新失敗，後端資料異常";
                this.Response.StatusCode = 400;
            }
            catch (Exception ex)
            {
                //更新失敗
                msg.code = 500; //Internal Server Error
                msg.status = "更新失敗，後端資料連線異常";
                this.Response.StatusCode = 500;
            }


            //回應也是一個Json內容
            return Json(msg);
        }
    }
}
