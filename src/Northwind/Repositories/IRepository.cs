using System.Linq;

namespace Northwind.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity Find(params object[] keys);
        IQueryable<TEntity> Query { get; }
        void SaveChanges();
    }
}
