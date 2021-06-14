using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Data.Models
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
