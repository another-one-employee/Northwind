using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Repositories
{
    public class EntityRepository<TDbEntity, TDomain> : IRepository<TDomain>
        where TDomain : class
        where TDbEntity : class
    {
        protected DbContext Context { get; }
        protected IMapper Mapper { get; }
        protected DbSet<TDbEntity> Set => Context.Set<TDbEntity>();
        public IQueryable<TDomain> Query => Context.Set<TDomain>();

        public EntityRepository(DbContext dbContext, IMapper mapper)
        {
            Context = dbContext;
            Mapper = mapper;
        }

        public void Update(TDomain entity)
        {
            TDbEntity dbEntity = Mapper.Map<TDbEntity>(entity);
            var entry = Context.Entry(dbEntity);
            if (entry.State == EntityState.Detached)
                Set.Attach(dbEntity);
            entry.State = EntityState.Modified;
        }

        public async Task InsertAsync(TDomain entity)
        {
            TDbEntity dbEntity = Mapper.Map<TDbEntity>(entity);
            var entry = Context.Entry(dbEntity);
            if (entry.State == EntityState.Detached)
                await Set.AddAsync(dbEntity);
        }

        public async Task<TDomain> FindAync(params object[] keys)
        {
            TDbEntity dbEntity = await Set.FindAsync(keys);
            return Mapper.Map<TDomain>(dbEntity);
        }

        public virtual async Task<IEnumerable<TDomain>> FindAllAync()
        {
            List<TDbEntity> allItems = await Set.ToListAsync();
            return Mapper.Map<List<TDbEntity>, IEnumerable<TDomain>>(allItems);
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
