using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    //POCO(Plain Old CLR Object)
    public class GjunHelloController
    {
        //打招呼
        public ContentResult helloWorld()
        {
            ContentResult content = new ContentResult()
            {
                Content = "<font size='6' color='red'>Hello Gjun</font>",
                ContentType = "text/html"
            };
            return content;
        }
    }
}
