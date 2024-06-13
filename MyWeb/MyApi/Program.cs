using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
//客製化logging
builder.Logging.ClearProviders(); //清除所有的Provider
//重新來過
builder.Logging.AddConsole(); //加入Console Provider
//加入一個File Provider
//builder.Logging.AddFile("log-{Date}.txt",LogLevel.Error); //加入File Provider

// Add services to the container.
//使用擴充方法 啟動控制器(Controllers) 服務 不支援View
builder.Services.AddControllers();
//透過WebApplicationBuilder 獲取到管理appsettings.json檔案(Config管理員)
ConfigurationManager manager = builder.Configuration;
//透過Config 管理員取得連接字串
String connectionString = manager.GetConnectionString("nor");
//使用泛型Metadata註冊DbContext 變成一個應用系統的服務
builder.Services.AddDbContext<NorthwndContext>(
    //Lambda 表達式 透過DbContextOptionsBuilder 進行設定
    (options) =>
    {
        options.UseSqlServer(connectionString);
    }
    );
//---------------------------取出系統設定可允許前端Site---------------------------
//取出系統設定可允許前端Site
String sites = manager.GetSection("corsallow").Get<String>();
//將字串以逗號分隔轉換成字串陣列
String[] siteArray = sites.Split(',');

//--------------CORS Policy----------------
builder.Services.AddCors(
    //Lambda 表達式 進行設定
    (options) =>
    {
        //設定CORS Policy 與名稱
        options.AddPolicy("all",
            //Lambda 表達式 進行設定
            (builder) =>
            {
                //允許所有來源
                builder.AllowAnyOrigin();
                //允許所有方法
                builder.AllowAnyMethod();
                //允許所有標頭
                builder.AllowAnyHeader();
            }
        );
        //定義特定Site Policy
        options.AddPolicy("sites",
            (builder) =>
            {
                //動態配置允許的前端Domain(配置採用appsettings.json)
                builder.WithOrigins(siteArray);
                builder.WithMethods("GET", "POST", "PUT", "DELETE");
                builder.AllowAnyHeader(); //允許所有的Hearder
            }
        );

    }
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    //客製化SwaggerOptions
    (options) =>
    {
        options.SwaggerDoc("v1", 
            //匿名型別(類別)
            new() { Title = "RESTful 服務-客服系統",Version = "v1", Description="客服系統API文件"}
            );
        //加入XML文件
        //動態取得配合應用系統名稱execute assembly name 取得XML文件
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //動態取得XML路徑
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //加入XML文件
        options.IncludeXmlComments(xmlPath);
    }
    ); //使用Open api swagger 產生API文件輔助員

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Middleware 使用all Policy
app.UseCors("sites");
app.UseAuthorization();
//app.UseStaticFiles(); 絕對不使用 並非網站系統 沒有靜態資源的URL
app.MapControllers(); //API端點具有彈性 可能目錄是參數 使用RouteAttribute Class (Declare定義型的類別) 語法糖架構
app.UseMiddleware<ErrorHandlerMiddleware>(); //使用自訂的Middleware
app.Run();
