using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Data Field
        private NorthwndContext _context;
        //自訂建構子(建構子注入依賴物件)
        public CustomersController(NorthwndContext context)
        {
            _context = context;
        }
        //提供傳入客戶資料(Json)進行客戶資料"新增"作業--Http request: POST
        //注入Customers Entity物件 進行封裝json文件的屬性
        //規範前端採用Http Request Content-Type:application/json
        //規範回應的Response Content-Type:application/json
        //針對swagger 產生的API文件，提供API文件的描述 Response規格
        [HttpPostAttribute]
        [RouteAttribute("add/rawdata")]
        [ConsumesAttribute("application/json")] //Request Content-Type
        [ProducesAttribute("application/json")] //Response Content-Type
        [ProducesResponseTypeAttribute(typeof(Message),StatusCodes.Status200OK)] //Response Status Code
        [ProducesResponseTypeAttribute(typeof(Message), StatusCodes.Status400BadRequest)]
        public IActionResult customersAdd(Customers customers) //注入 Injection 物件
        {
            //進行客戶資料新增作業(第一個階段 只是加入物件到Persistant Context)
            _context.Customers.Add(customers);
            Message message = new Message();
            //同步更新到資料庫Sql Server Dialect(產生Native SQL)
            try
            {
                Int32 row = _context.SaveChanges();
                message.code = 200;
                message.message = "新增客戶資料成功";
                //回覆訊息到使用者端(應用系統) and Http Status Code-200
                return Ok(message);
            }
            catch(DbUpdateException ex)
            {
                //回覆訊息到使用者端(應用系統) and Http Status Code-400
                message.code = 400;
                message.message = "新增客戶資料失敗！可能編號重複了！！";
                return BadRequest(message);　//400
            }

            
        }

        //找出所有客戶資料清單
        [HttpGetAttribute]
        [RouteAttribute("all/rawdata")]
        [ProducesAttribute("application/json")]
        public IActionResult customersAll()
        {
            //LINQ for Entity Framework Core
            var result=(from c in _context.Customers
                        select c).ToList();
            return new OkObjectResult(result); //序列化成JSON文件 Http Status Code 200
        }
    }
}
