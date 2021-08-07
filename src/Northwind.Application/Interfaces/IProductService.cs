using Northwind.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Application.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductEntity productEntityDb);
        Task UpdateAsync(ProductEntity productEntityDb);
        Task<ProductEntity> GetByIdAsync(int id);
        Task<IEnumerable<ProductEntity>> GetMaxAmountAsync(int maxAmountOfProducts);
        Task DeleteAsync(ProductEntity productEntityDb);
        Task<IEnumerable<SupplierEntity>> GetSuppliers();
        Task<IEnumerable<CategoryEntity>> GetCategories();
    }
}