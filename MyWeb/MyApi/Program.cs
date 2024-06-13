using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
//�Ȼs��logging
builder.Logging.ClearProviders(); //�M���Ҧ���Provider
//���s�ӹL
builder.Logging.AddConsole(); //�[�JConsole Provider
//�[�J�@��File Provider
//builder.Logging.AddFile("log-{Date}.txt",LogLevel.Error); //�[�JFile Provider

// Add services to the container.
//�ϥ��X�R��k �Ұʱ��(Controllers) �A�� ���䴩View
builder.Services.AddControllers();
//�z�LWebApplicationBuilder �����޲zappsettings.json�ɮ�(Config�޲z��)
ConfigurationManager manager = builder.Configuration;
//�z�LConfig �޲z�����o�s���r��
String connectionString = manager.GetConnectionString("nor");
//�ϥΪx��Metadata���UDbContext �ܦ��@�����Ψt�Ϊ��A��
builder.Services.AddDbContext<NorthwndContext>(
    //Lambda ��F�� �z�LDbContextOptionsBuilder �i��]�w
    (options) =>
    {
        options.UseSqlServer(connectionString);
    }
    );
//---------------------------���X�t�γ]�w�i���\�e��Site---------------------------
//���X�t�γ]�w�i���\�e��Site
String sites = manager.GetSection("corsallow").Get<String>();
//�N�r��H�r�����j�ഫ���r��}�C
String[] siteArray = sites.Split(',');

//--------------CORS Policy----------------
builder.Services.AddCors(
    //Lambda ��F�� �i��]�w
    (options) =>
    {
        //�]�wCORS Policy �P�W��
        options.AddPolicy("all",
            //Lambda ��F�� �i��]�w
            (builder) =>
            {
                //���\�Ҧ��ӷ�
                builder.AllowAnyOrigin();
                //���\�Ҧ���k
                builder.AllowAnyMethod();
                //���\�Ҧ����Y
                builder.AllowAnyHeader();
            }
        );
        //�w�q�S�wSite Policy
        options.AddPolicy("sites",
            (builder) =>
            {
                //�ʺA�t�m���\���e��Domain(�t�m�ĥ�appsettings.json)
                builder.WithOrigins(siteArray);
                builder.WithMethods("GET", "POST", "PUT", "DELETE");
                builder.AllowAnyHeader(); //���\�Ҧ���Hearder
            }
        );

    }
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    //�Ȼs��SwaggerOptions
    (options) =>
    {
        options.SwaggerDoc("v1", 
            //�ΦW���O(���O)
            new() { Title = "RESTful �A��-�ȪA�t��",Version = "v1", Description="�ȪA�t��API���"}
            );
        //�[�JXML���
        //�ʺA���o�t�X���Ψt�ΦW��execute assembly name ���oXML���
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //�ʺA���oXML���|
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //�[�JXML���
        options.IncludeXmlComments(xmlPath);
    }
    ); //�ϥ�Open api swagger ����API��󻲧U��

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Middleware �ϥ�all Policy
app.UseCors("sites");
app.UseAuthorization();
//app.UseStaticFiles(); ���藍�ϥ� �ëD�����t�� �S���R�A�귽��URL
app.MapControllers(); //API���I�㦳�u�� �i��ؿ��O�Ѽ� �ϥ�RouteAttribute Class (Declare�w�q�������O) �y�k�}�[�c
app.UseMiddleware<ErrorHandlerMiddleware>(); //�ϥΦۭq��Middleware
app.Run();
