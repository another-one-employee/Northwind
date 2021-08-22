using AutoMapper;
using Northwind.Application.Exceptions;
using Northwind.Application.Interfaces;
using Northwind.Application.Models;
using Northwind.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductAsyncRepository _productRepository;
        private readonly IAsyncRepository<Supplier> _supplierRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductAsyncRepository productRepository,
            IAsyncRepository<Supplier> supplierRepository,
            IAsyncRepository<Category> categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductEntity>> GetMaxAmountAsync(int maxAmountOfProducts)
        {
            if (maxAmountOfProducts == 0)
            {
                var allModels = await _productRepository.FindAllAsync();
                return _mapper.Map<IEnumerable<ProductEntity>>(allModels);
            }

            var someModels = await _productRepository.TakeLast(maxAmountOfProducts);
            return _mapper.Map<IEnumerable<ProductEntity>>(someModels);
        }

        public async Task CreateAsync(ProductEntity productEntity)
        {
            var model = _mapper.Map<Models.Product>(productEntity);
            await _productRepository.InsertAsync(model);
        }

        public async Task<ProductEntity> GetByIdAsync(int id)
        {
            var model = await _productRepository.FindAsync(p => p.ProductId == id) ??
                        throw new NotFoundException(nameof(ProductEntity), id);

            return _mapper.Map<ProductEntity>(model);
        }

        public async Task UpdateAsync(ProductEntity productEntity)
        {
            var model = _mapper.Map<Models.Product>(productEntity);
            await _productRepository.UpdateAsync(model);
        }

        public async Task DeleteAsync(ProductEntity productEntity)
        {
            var model = _mapper.Map<Models.Product>(productEntity);
            await _productRepository.DeleteAsync(model);
        }

        public async Task<IEnumerable<SupplierEntity>> GetSuppliers()
        {
            var models = await _supplierRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<SupplierEntity>>(models);
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategories()
        {
            var models = await _categoryRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<CategoryEntity>>(models);
        }
    }
}
