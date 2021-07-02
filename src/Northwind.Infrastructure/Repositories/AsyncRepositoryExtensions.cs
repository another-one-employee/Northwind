using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Repositories
{
    public static class AsyncRepositoryExtensions
    {
        public static async Task<TEntity> FindAsync<TEntity>(this IRepository<TEntity> repository, params object[] keys)
            where TEntity : class
        {
            return repository.Query is DbSet<TEntity> set ? await set.FindAsync(keys) : null;
        }

        public static async Task InsertAsync<TEntity>(this IRepository<TEntity> repository, TEntity entity)
            where TEntity : class
        {
            var set = repository.Query as DbSet<TEntity>;

            await set.AddAsync(entity);
        }
    }
}
