using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetMaxAmountAsync(int maxAmountOfProducts);
        Task DeleteAsync(Product product);
        Task<IEnumerable<Supplier>> GetSuppliers();
        Task<IEnumerable<Category>> GetCategories();
    }
}