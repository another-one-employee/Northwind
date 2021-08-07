using System;
using AutoMapper;
using Northwind.Core.Exceptions;
using Northwind.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Core.Models;
using Product = Northwind.Core.Entities.Product;

namespace Northwind.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IAsyncRepository<Models.Product> _productRepository;
        private readonly IAsyncRepository<Supplier> _supplierRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IAsyncRepository<Models.Product> productRepository,
            IAsyncRepository<Supplier> supplierRepository,
            IAsyncRepository<Category> categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetMaxAmountAsync(int maxAmountOfProducts)
        {
            var models = await _productRepository.FindAllAsync();

            if (maxAmountOfProducts == 0)
            {
                return _mapper.Map<IEnumerable<Product>>(models);
            }

            return _mapper.Map<IEnumerable<Product>>(models.Take(maxAmountOfProducts));
        }

        public async Task CreateAsync(Product product)
        {
            var model = _mapper.Map<Models.Product>(product);
            await _productRepository.InsertAsync(model);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var model = await _productRepository.FindAsync(p => p.ProductId == id) ??
                        throw new NotFoundException(nameof(Product), id);

            return _mapper.Map<Product>(model);
        }

        public async Task UpdateAsync(Product product)
        {
            var model = _mapper.Map<Models.Product>(product);
            await _productRepository.UpdateAsync(model);
        }

        public async Task DeleteAsync(Product product)
        {
            var model = _mapper.Map<Models.Product>(product);
            await _productRepository.DeleteAsync(model);
        }

        public async Task<IEnumerable<Entities.Supplier>> GetSuppliers()
        {
            var models = await _supplierRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<Entities.Supplier>>(models);
        }

        public async Task<IEnumerable<Entities.Category>> GetCategories()
        {
            var models = await _categoryRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<Entities.Category>>(models);
        }
    }
}
