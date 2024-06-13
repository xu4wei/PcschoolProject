using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//圖檔上傳或者下載操作
namespace MyWeb.Controllers
{
    public class ImageFileController : Controller
    {
        //Data Field
        private readonly IWebHostEnvironment _webEnvironment;

        //建構子注入依賴物件(Dependency Injection-DI軟體工程)
        //注入網站系統底層的環境物件(目的在於透過虛擬目錄 取得磁碟實地目錄等操作)
        public ImageFileController(IWebHostEnvironment _webEnvironment)
        {
            this._webEnvironment = _webEnvironment;
            Console.WriteLine("ImageFileController建構子被呼叫..." + this._webEnvironment.WebRootPath);
        }

        public IActionResult showFile(String file)
        {
            //1.反映實際目錄
            String realPath = Path.Combine(this._webEnvironment.WebRootPath,"images");
            //第一次請求 採用Http request Method:GET
            if (this.Request.Method.Equals("POST"))
            {
                //進行檔案下載
                //1.反映實際目錄+檔案名稱
                //String fullName = realPath + "/" + file;
                //1.Virtual Path
                String Vpath = "/images/" + file;
                //2.進行檔案下載
                return this.File(Vpath,"image/jpeg"); //Response Header Content-Type 稱呼為 MIME Type
            }
            //產生下拉式功能表集合清單項目List<SelectListItem>
            //如何獲取網站資料夾images下的所有圖檔(如何將虛擬目錄轉換成實際目錄)
            
            //取出這一個實際目錄所有檔案
            String[] files = Directory.GetFiles(realPath);
            //走訪每一個檔案名稱->建構成FileInfor物件
            //建構集合物件
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(String f in files) //走訪每一個完整性檔案名稱
            {
                //建構一個FileInfo物件 取出檔案屬性(Name Property 只有檔案名稱.副檔名)
                String fileName = new FileInfo(f).Name;
                Console.WriteLine(fileName);
                //建構SelectListItem物件
                String[] items = fileName.Split(new Char[] {'.'}); //分割檔案名稱與副檔名
                SelectListItem item = new SelectListItem(items[0],fileName);
                list.Add(item);
            }
            //將狀態持續到頁面 ViewBag屬性是一個動態型別 dynamic
            this.ViewBag.files = list;


            return View();
        }
    }
}
