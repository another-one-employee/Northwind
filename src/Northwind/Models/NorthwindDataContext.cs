using Microsoft.EntityFrameworkCore;

namespace Northwind.Models
{
    public class NorthwindDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }

        public NorthwindDataContext(DbContextOptions<NorthwindDataContext> options)
            : base(options) { }
    }
}
