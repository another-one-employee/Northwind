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

        public override async Task<ProductDTO> FindAsync(params object[] keys)
        {
            Product dbEntity = await Set.FindAsync(keys);

            if (dbEntity != null)
            {
                await Context.Entry(dbEntity).Reference(s => s.Supplier).LoadAsync();
                await Context.Entry(dbEntity).Reference(c => c.Category).LoadAsync();

                Context.Entry(dbEntity).State = EntityState.Detached;
                Context.Entry(dbEntity.Category).State = EntityState.Detached;
                Context.Entry(dbEntity.Supplier).State = EntityState.Detached;
            }

            return Mapper.Map<ProductDTO>(dbEntity);
        }

        public override async Task<IEnumerable<ProductDTO>> FindAllAsync()
        {
            List<Product> allItems = await Set
               .Include(s => s.Supplier)
               .Include(c => c.Category)
               .AsNoTracking()
               .ToListAsync();

            return Mapper.Map<List<Product>, IEnumerable<ProductDTO>>(allItems);
        }
    }
}
