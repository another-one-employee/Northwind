using Northwind.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Application.Interfaces
{
    public interface IProductAsyncRepository : IAsyncRepository<Product>
    {
        Task<IEnumerable<Product>> TakeLast(int count);
    }
}
