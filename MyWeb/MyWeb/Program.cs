//Top Level Statements(Entry Point--�D�{��)
//�g�Ѻ����u�t�إߤ@��WebApplicationBuilder����(builder)
using Microsoft.EntityFrameworkCore;
using MyWeb.DbModels;
using MyWeb.Models;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//�[�JMVC�A�Ȩ�IServiceCollection ���� �e��
builder.Services.AddControllersWithViews(
    //�ϥ�Lambda ��{Action delegate�[�c�U��functional
    (options) =>
    {
        //�G�pFilter�d�I��
        options.Filters.Add<MyWeb.Models.ApiKeyFilter>();  //�ϥΪx��
    }
); //MVC

//builder.Services.AddControllers(); //WebAPI

//builder.Services.AddAuthentication(....); ��ϥ�Middleware UseAuthentication() �t�X�ϥ�

//�ۭq�X�R�i�Ӫ�Service(����-Model/�Ϊ̸�Ʀs������...)
//1.���o�պA�s���r��(�����ϥ�XML�պA)ConfigurationManager�[�c �w��JSON Property ����SectionInfo
ConfigurationManager manager = builder.Configuration;
//2.���o�s���r��(��Ʈw�s���r��)
String connectionString = manager.GetConnectionString("nor");
//3.�إ߳s������(�����h����) �ݥ~��System.Data.SqlClient
IDbConnection connection = new SqlConnection(connectionString);
//4.�غc�ۭqCustomers DAO �`�J�̿����Y�s������
CustomersDao dao = new CustomersDao()
{
    Connection = connection //Property Injection--�ݩʪ`�J�̿ફ��
}; //Object Initializer �����l�ƾ�
//5.�[�J�A�Ȯe��(�u�t�Ҧ� Class Factory Pattern)
IServiceCollection services = builder.Services;
//�C���ШD���ͤ@�ӿW�ߪ�����
services.AddScoped<IDao<MyWeb.DbModels.Customers, String>>(serviceProvider=>dao); //�x�����O�`�J

//---------------�Ȼs��ApiConfig Instance����---------------
ApiConfig apiConfig = new ApiConfig();
//�z�LConfigurationManagerŪ��appsettings.json �ۭq�ݩ�
IConfigurationSection secton = manager.GetSection("apiconfig");
//�j�w�ӰϬq���ݩ� �`�J����۹��ݩʤ�
secton.Bind(apiConfig);
//�[�J�A�Ȯe��(�u�t�Ҧ� Class Factory Pattern) Singleton�ͩR�g��
services.AddSingleton<ApiConfig>(apiConfig);

//-----------------�Ȼs��CustomersApi Instance ���U��service----------------
CustomersApi customersApi = new CustomersApi();
//�z�LConfigurationManagerŪ��appsettings.json �ۭq�ݩ�
IConfigurationSection customersApiSection = manager.GetSection("customersApi");
//�j�w�ӰϬq���ݩ� �`�J����۹��ݩʤ�
customersApiSection.Bind(customersApi);
//�[�J�A�Ȯe��(�u�t�Ҧ� Class Factory Pattern) Singleton�ͩR�g��
services.AddSingleton<CustomersApi>(customersApi);

//-----�[�JOrderAPI�A��-----
OrderApi orderApi = new OrderApi();
//�z�LConfigurationManagerŪ��appsettings.json �ۭq�ݩ�
IConfigurationSection orderApiSection = manager.GetSection("ordersApi");
//�j�w�ӰϬq���ݩ� �`�J����۹��ݩʤ�
orderApiSection.Bind(orderApi);
//�[�J�A�Ȯe��(�u�t�Ҧ� Class Factory Pattern)Singleton �ͩR�g��
services.AddSingleton<OrderApi>(orderApi);


//�[�J�ۭq��DbContext �A��(Scoped�ͩR�g��)
services.AddDbContext<NorthwndContext>(options =>
{
    options.UseSqlServer(connectionString); //���ͤ@��DbContextOptions����(�ϥγs���r��)
});
//�[�JHttpClient�A��(�]���w�]�O����)
builder.Services.AddHttpClient(); //HttpClient�A��

//�g��builder�إߤ@��WebApplication����(app)
var app = builder.Build();

// Configure the HTTP request pipeline.(Middleware)�㦳���ǩ�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//�ϥξɦV��X
app.UseHttpsRedirection();
//�����n�ϥ��R�A��� static file(HTML,CSS,JavaScript,image,etc.)--�w�]�ؿ��N�Owwwroot
app.UseStaticFiles();//�ŰѼƹw�]�����R�A��Ƨ����� �Owwwroot
//�bMiddleware(�����n��)���[�J�@�Ӹ��ѳ̨Ϊ��t�X �ϥ�Controller class and method �ϥ�Attribute �ݩʴy�z����
app.UseRouting();

//�Ұʨt�ζi����v�覡�t�m(Bearer, JWT, Cookie, OpenID Connect, OAuth2.0)
app.UseAuthorization();

//�b�������Ψt�Ϊ���[�J�@��Middleware(�����n��)�H�B�zHTTP�ШD(�i����Ѱt�m -- URL Pattern)
//Middleware(�����n��)�O�㦳���ǩʪ�
app.MapControllerRoute(
    //��W�ѼƼg�k
    name: "default", //���ѦW��(�ۭq)
    pattern: "{controller=Home}/{action=Index}/{id?}");
//�t�m�ĤG�Ӹ��ѳW�h(���K�s�D)
app.MapControllerRoute(
    name:"gjun",
    pattern:"gjun/{*news}",
    defaults: new {controller="gjun",action="news"} //�غc�@�ӰΦW��������(�ϥ�{�ݩ�=��,...})
    );
//�����t�αҰ�
app.Run();
