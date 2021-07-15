using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Models;
using Northwind.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Repositories
{
    public class ProductRepository : EntityRepository<Product, ProductDTO>
    {
        public ProductRepository(DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public override async Task<IEnumerable<ProductDTO>> FindAllAsync()
        {
            List<Product> allItems = await Set
               .Include(s => s.Supplier)
               .Include(c => c.Category)
               .ToListAsync();

            return Mapper.Map<List<Product>, IEnumerable<ProductDTO>>(allItems);
        }
    }
}
