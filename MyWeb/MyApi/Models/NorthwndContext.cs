using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    public class NorthwndContext:DbContext
    {
        //自訂建構子 指向DbContextOptions 讓系統啟動外掛連接字串
        public NorthwndContext(DbContextOptions<NorthwndContext> options):base(options)
        {
        }
        //規劃屬性 DbSet應對資料表屬性設定(屬性名稱對應資料表名稱)
        public DbSet<Customers> Customers { set; get; }
        public DbSet<Orders> VWCustomersOrders { set; get; }
        public DbSet<OrderDetails> VwDetailsProducts { set; get; }
        public DbSet<OrderDetailsTB> OrderDetails {  set; get; }

        //覆寫OnModelCreating方法
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //因為OrderDetailsTB KeyAttribute複合鍵
            modelBuilder.Entity<OrderDetailsTB>().HasKey((od) => new { od.OrderID, od.ProductID });
        }
    }
}
