using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Northwind.Application.Interfaces;

namespace Northwind.Infrastructure.Repositories
{
    public class EntityRepository<TEntity> : IAsyncRepository<TEntity>
        where TEntity : class
    {
        protected DbContext Context { get; }
        protected DbSet<TEntity> Set => Context.Set<TEntity>();

        public EntityRepository(DbContext dbContext)
        {
            Context = dbContext;
        }
        public async Task InsertAsync(TEntity entity)
        {
            await Set.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Set.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set.AsNoTracking().FirstAsync(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await Set.AsNoTracking().ToListAsync();
        }
    }
}
