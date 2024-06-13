using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;

namespace MyWeb.Controllers
{
    public class OrdersClientController : Controller
    {
        //注入OrderApi
        private readonly OrderApi _orderApi;
        public OrdersClientController(OrderApi orderApi)
        {
            _orderApi = orderApi;
        }
        //進行查詢訂單頁面調用
        public IActionResult ordersQry()
        {
            //持續狀態OrdeApi到View Page去
            ViewData["qryurl"] = _orderApi.qryurl;
            ViewData["updateurl"] = _orderApi.updateurl;
            ViewData["deleteurl"] = _orderApi.deleteurl;
            return View();
        }
        
    }
}
