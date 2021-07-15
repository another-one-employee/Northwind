using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> FindAsync(params object[] keys);
        Task<IEnumerable<TEntity>> FindAllAsync();
    }
}
