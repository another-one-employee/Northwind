using Northwind.Core.Exceptions;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IAsyncRepository<ProductDTO> _productRepository;

        public ProductService(IAsyncRepository<ProductDTO> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDTO>> GetMaxAmountAsync(int maxAmountOfProducts)
        {
            var products = await _productRepository.FindAllAsync();

            if (maxAmountOfProducts == 0)
            {
                return products;
            }

            return products.Take(maxAmountOfProducts);
        }

        public async Task CreateAsync(ProductDTO product)
        {
            await _productRepository.InsertAsync(product);
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            return await _productRepository.FindAsync(id) ?? throw new NotFoundException(nameof(ProductDTO), id);
        }

        public async Task UpdateAsync(ProductDTO product)
        {
            await _productRepository.UpdateAsync(product);
        }
    }
}
