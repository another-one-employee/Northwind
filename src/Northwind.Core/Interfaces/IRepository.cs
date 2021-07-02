using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> FindAync(params object[] keys);
        Task<IEnumerable<TEntity>> FindAllAync();
        IQueryable<TEntity> Query { get; }
        Task SaveChangesAsync();
    }
}
