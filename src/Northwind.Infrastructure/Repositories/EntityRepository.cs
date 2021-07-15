using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Repositories
{
    public class EntityRepository<TDbModel, TDomain> : IAsyncRepository<TDomain>
        where TDomain : class
        where TDbModel : class
    {
        protected DbContext Context { get; }
        protected IMapper Mapper { get; }
        protected DbSet<TDbModel> Set => Context.Set<TDbModel>();

        public EntityRepository(DbContext dbContext, IMapper mapper)
        {
            Context = dbContext;
            Mapper = mapper;
        }

        public async Task UpdateAsync(TDomain entity)
        {
            TDbModel dbEntity = Mapper.Map<TDbModel>(entity);

            var entry = Context.Entry(dbEntity);
            if (entry.State == EntityState.Detached)
                Set.Attach(dbEntity);
            entry.State = EntityState.Modified;

            await Context.SaveChangesAsync();
        }

        public async Task InsertAsync(TDomain entity)
        {
            TDbModel dbEntity = Mapper.Map<TDbModel>(entity);
            var entry = Context.Entry(dbEntity);
            if (entry.State == EntityState.Detached)
                await Set.AddAsync(dbEntity);

            await Context.SaveChangesAsync();
        }

        public async Task<TDomain> FindAsync(params object[] keys)
        {
            TDbModel dbEntity = await Set.FindAsync(keys);
            return Mapper.Map<TDomain>(dbEntity);
        }

        public virtual async Task<IEnumerable<TDomain>> FindAllAsync()
        {
            List<TDbModel> allItems = await Set.ToListAsync();
            return Mapper.Map<List<TDbModel>, IEnumerable<TDomain>>(allItems);
        }
    }
}
