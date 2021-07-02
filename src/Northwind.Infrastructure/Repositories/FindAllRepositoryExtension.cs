using System.Linq;

namespace Northwind.Infrastructure.Repositories
{
    public static class FindAllRepositoryExtension
    {
        public static IQueryable<TEntity> FindAll<TEntity>(this IRepository<TEntity> repository)
            where TEntity : class
        {
            return repository.Query;
        }
    }
}
