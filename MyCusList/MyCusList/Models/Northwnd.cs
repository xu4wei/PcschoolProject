using System.Data.Entity;

namespace MyCusList.Models
{
    public partial class Northwnd : DbContext
    {
        public Northwnd()
            : base("name=nor")
        { }

        public virtual DbSet<Customers> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>()
                .Property(e => e.CustomerID)
                .IsFixedLength();

        }
    }
}
