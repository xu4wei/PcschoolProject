//Top Level Statements(Entry Point--主程式)
//經由網站工廠建立一個WebApplicationBuilder物件(builder)
using Microsoft.EntityFrameworkCore;
using MyWeb.DbModels;
using MyWeb.Models;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//加入MVC服務到IServiceCollection 介面 容器
builder.Services.AddControllersWithViews(
    //使用Lambda 實現Action delegate架構下的functional
    (options) =>
    {
        //佈署Filter攔截器
        options.Filters.Add<MyWeb.Models.ApiKeyFilter>();  //使用泛型
    }
); //MVC

//builder.Services.AddControllers(); //WebAPI

//builder.Services.AddAuthentication(....); 跟使用Middleware UseAuthentication() 配合使用

//自訂擴充進來的Service(元件-Model/或者資料存取元件...)
//1.取得組態連接字串(早期使用XML組態)ConfigurationManager架構 針對JSON Property 應對SectionInfo
ConfigurationManager manager = builder.Configuration;
//2.取得連接字串(資料庫連接字串)
String connectionString = manager.GetConnectionString("nor");
//3.建立連接物件(介面多型化) 需外掛System.Data.SqlClient
IDbConnection connection = new SqlConnection(connectionString);
//4.建構自訂Customers DAO 注入依賴關係連接物件
CustomersDao dao = new CustomersDao()
{
    Connection = connection //Property Injection--屬性注入依賴物件
}; //Object Initializer 物件初始化器
//5.加入服務容器(工廠模式 Class Factory Pattern)
IServiceCollection services = builder.Services;
//每次請求產生一個獨立的物件
services.AddScoped<IDao<MyWeb.DbModels.Customers, String>>(serviceProvider=>dao); //泛型型別注入

//---------------客製化ApiConfig Instance物件---------------
ApiConfig apiConfig = new ApiConfig();
//透過ConfigurationManager讀取appsettings.json 自訂屬性
IConfigurationSection secton = manager.GetSection("apiconfig");
//綁定該區段的屬性 注入物件相對屬性中
secton.Bind(apiConfig);
//加入服務容器(工廠模式 Class Factory Pattern) Singleton生命週期
services.AddSingleton<ApiConfig>(apiConfig);

//-----------------客製化CustomersApi Instance 註冊為service----------------
CustomersApi customersApi = new CustomersApi();
//透過ConfigurationManager讀取appsettings.json 自訂屬性
IConfigurationSection customersApiSection = manager.GetSection("customersApi");
//綁定該區段的屬性 注入物件相對屬性中
customersApiSection.Bind(customersApi);
//加入服務容器(工廠模式 Class Factory Pattern) Singleton生命週期
services.AddSingleton<CustomersApi>(customersApi);

//-----加入OrderAPI服務-----
OrderApi orderApi = new OrderApi();
//透過ConfigurationManager讀取appsettings.json 自訂屬性
IConfigurationSection orderApiSection = manager.GetSection("ordersApi");
//綁定該區段的屬性 注入物件相對屬性中
orderApiSection.Bind(orderApi);
//加入服務容器(工廠模式 Class Factory Pattern)Singleton 生命週期
services.AddSingleton<OrderApi>(orderApi);


//加入自訂的DbContext 服務(Scoped生命週期)
services.AddDbContext<NorthwndContext>(options =>
{
    options.UseSqlServer(connectionString); //產生一個DbContextOptions物件(使用連接字串)
});
//加入HttpClient服務(因為預設是關閉)
builder.Services.AddHttpClient(); //HttpClient服務

//經由builder建立一個WebApplication物件(app)
var app = builder.Build();

// Configure the HTTP request pipeline.(Middleware)具有順序性
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//使用導向輸出
app.UseHttpsRedirection();
//網站要使用靜態資料 static file(HTML,CSS,JavaScript,image,etc.)--預設目錄就是wwwroot
app.UseStaticFiles();//空參數預設對應靜態資料夾的根 是wwwroot
//在Middleware(中介軟體)中加入一個路由最佳的配合 使用Controller class and method 使用Attribute 屬性描述路由
app.UseRouting();

//啟動系統進行授權方式配置(Bearer, JWT, Cookie, OpenID Connect, OAuth2.0)
app.UseAuthorization();

//在網站應用系統物件加入一個Middleware(中介軟體)以處理HTTP請求(進行路由配置 -- URL Pattern)
//Middleware(中介軟體)是具有順序性的
app.MapControllerRoute(
    //具名參數寫法
    name: "default", //路由名稱(自訂)
    pattern: "{controller=Home}/{action=Index}/{id?}");
//配置第二個路由規則(巨匠新聞)
app.MapControllerRoute(
    name:"gjun",
    pattern:"gjun/{*news}",
    defaults: new {controller="gjun",action="news"} //建構一個匿名類型物件(使用{屬性=值,...})
    );
//網站系統啟動
app.Run();
