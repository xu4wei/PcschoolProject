using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;


namespace MyApi.Controllers
{
    /// <summary>
    /// 訂單服務
    /// </summary>
    [RouteAttribute("api/v1/[controller]")]
    [ApiController]
    
    public class OrdersController : ControllerBase
    {
        //Data Field
        private readonly NorthwndContext _context;
        //建構子注入依賴物件(DI)
        public OrdersController(NorthwndContext context) 
        {
            _context = context;
        }

        //提供訂單主檔與明細檔查詢
        /// <summary>
        /// 訂單查詢作業_訂單主檔與明細檔查詢
        /// </summary>
        /// <param name="orderId">訂單編號</param>
        /// <returns>訂單主檔與明細檔JSON文件</returns>
        [HttpGetAttribute] //Request Method-GET
        [RouteAttribute("ordersqry/{oid}/rawdata")] //Request URL
        [ProducesAttribute("application/json")]
        [ProducesResponseTypeAttribute(typeof(Order_Details),200)]
        [ProducesResponseTypeAttribute(typeof(Message),400)]
        [ApiKeyFilterAttribute()]
        public IActionResult ordersQry([FromRoute(Name ="oid")]Int32 orderId)
        {
            //借助注入進來的DbContext 配合LINQ語法查詢 訂單主檔與明細檔資訊查詢
            var orders = (from o in _context.VWCustomersOrders
                          where o.OrderID == orderId
                          select o).FirstOrDefault();
            if (orders == null)
            {
                Message message = new Message()
                {
                    code = 400,
                    message = $"查無該訂單:{orderId} 資料"
                };
                return this.BadRequest(message); //Http status code 400
            }
            else
            {
                //找出訂單明細
                var details = (from d in _context.VwDetailsProducts
                              where d.OrderID == orderId
                              select d).ToList();
                //建構一個窗口物件 一對多關聯
                Order_Details order_Details = new Order_Details()
                {
                    orders = orders,
                    orderDetails = details
                }; //Object Initializer
                return this.Ok(order_Details); //Http status code 200
            }
            
        }

        //傳遞訂單明細資料進來 進行相對訂單明細修改作業
        //進入一個OrderDetailsTB物件 看看傳進來的json屬性對應與封裝
        [HttpPutAttribute]
        [RouteAttribute("orderdetails/update")]
        [ProducesAttribute("application/json")]
        [ConsumesAttribute("application/json")]
        [ProducesResponseType(typeof(Message),200)]
        [ProducesResponseType(typeof(Message),400)]
        public Message orderDetailsUpdate(OrderDetailsTB details)
        {
            Message message = new Message();
            //注入DbContext 進行資料庫更新
            //先將Entity進入於DbContext(ORM-Persistence持久層)--該Entity狀態為Add--同步資料庫產生Insert Into...
            _context.OrderDetails.Add(details);
            //事後調整該Entity EntityState狀態為Modified--同步資料庫產生Update...
            _context.Entry<MyApi.Models.OrderDetailsTB>(details).State = EntityState.Modified;
            //同步更新回資料庫
            try 
            {
                _context.SaveChanges();
                //更新成功
                this.Response.StatusCode = 200;
                message.code = 200;
                message.message = $"訂單明細:{details.OrderID} {details.ProductID} 更新成功";
            }
            catch(DbUpdateException ex) 
            {
                //調整Response Status Code
                this.Response.StatusCode = 400;
                message.code = 400;
                message.message = $"訂單明細:{details.OrderID} {details.ProductID} 更新失敗";
            }
            catch (Exception ex) 
            {
                //調整Response Status Code
                this.Response.StatusCode = 400;
                message.code = 400;
                message.message = $"訂單明細:{details.OrderID} {details.ProductID} 更新失敗";
            }
            return message;
        }

        //訂單明細的刪除作業
        [HttpDeleteAttribute]
        [RouteAttribute("orderDetails/delete/{oid}/{pid}")]
        [ProducesAttribute("application/json")]
        [ProducesResponseTypeAttribute(typeof(Message),200)]
        [ProducesResponseTypeAttribute(typeof(Message),400)]
        public IActionResult orderDetailsDelete([FromRouteAttribute(Name = "oid")]Int32 orderID,[FromRouteAttribute(Name = "pid")] Int32 ProductID)
        {
            Message message = new Message();
            //進行資料庫訂單明細刪除--先查詢 產生離線模組Entity物件
            var result = (from o in _context.OrderDetails
                          where o.OrderID == orderID && o.ProductID == ProductID
                          select o).FirstOrDefault();
            if (result == null)
            {
                //回應一個錯誤訊息(訂單明細不存在)
                message.code = 400;
                message.message = $"訂單明細:{orderID} 產品編號:{ProductID} 不存在";
                return this.BadRequest(message); // Http status code 400
            }
            else
            {
                //改變EntityState狀態為Deleted 在同步更新回資料庫
                _context.Entry<MyApi.Models.OrderDetailsTB>(result).State = EntityState.Deleted;
                //同步更新回資料庫
                try
                {
                    Int32 row = _context.SaveChanges();
                    //刪除成功
                    message.code = 200;
                    message.message = $"訂單明細:{orderID} 產品編號:{ProductID} 刪除成功";
                    return this.Ok(message); // Http status code 200
                }
                catch (DbUpdateException ex) 
                {
                    //刪除不動
                    message.code = 400;
                    message.message = $"訂單明細:{orderID} 產品編號:{ProductID} 刪除失敗";
                    return this.BadRequest(message); // Http status code 400
                }
                catch(Exception ex)
                {
                    //刪除不動
                    message.code = 400;
                    message.message = $"訂單明細:{orderID} 產品編號:{ProductID} 刪除失敗";
                    return this.BadRequest(message); // Http status code 400
                }
            }
        }
    }
}
