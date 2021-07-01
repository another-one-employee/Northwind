using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Northwind.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected DbContext Context { get; }

        protected DbSet<TEntity> Set => Context.Set<TEntity>();

        public IQueryable<TEntity> Query => Set;

        public EntityRepository(DbContext dbContext)
        {
            Context = dbContext;
        }

        public TEntity Find(params object[] keys)
        {
            return Set.Find(keys);
        }

        public void Insert(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            Set.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
