using Microsoft.EntityFrameworkCore;

namespace MyWeb.DbModels
{
    public class NorthwndContext:DbContext
    {
        //自訂建構子(借助DbContextOptions配置連接字串)
        public NorthwndContext(DbContextOptions<NorthwndContext> options) : base(options) 
        {
            //無須設計
        }

        //描述應對資料表DbSet Property 名稱對應資料表名稱
        public DbSet<Customers> Customers { get; set; }

    }
}
