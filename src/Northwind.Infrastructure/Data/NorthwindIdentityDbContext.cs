using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Infrastructure.Data
{
    public sealed class NorthwindIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public NorthwindIdentityDbContext(DbContextOptions<NorthwindIdentityDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
