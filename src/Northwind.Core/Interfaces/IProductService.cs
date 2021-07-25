using Northwind.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductDTO product);
        Task UpdateAsync(ProductDTO product);
        Task<ProductDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetMaxAmountAsync(int maxAmountOfProducts);
        Task DeleteAsync(ProductDTO product);
    }
}