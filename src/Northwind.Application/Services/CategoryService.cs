using AutoMapper;
using Northwind.Application.Exceptions;
using Northwind.Application.Interfaces;
using Northwind.Application.Models;
using Northwind.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IAsyncRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
        {
            var models = await _categoryRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<CategoryEntity>>(models);
        }

        public async Task<CategoryEntity> GetByIdAsync(int id)
        {
            var model = await FindByIdAsync(id);
            return _mapper.Map<CategoryEntity>(model);
        }

        public async Task<byte[]> GetPictureByIdAsync(int id)
        {
            var category = await GetByIdAsync(id);

            return category.Picture;
        }

        public async Task EditImageById(int id, byte[] image)
        {
            var model = await FindByIdAsync(id);

            model.Picture = image;
            await _categoryRepository.UpdateAsync(model);
        }

        private async Task<Category> FindByIdAsync(int id)
        {
            return await _categoryRepository.FindAsync(c => c.CategoryId == id) ??
                   throw new NotFoundException(nameof(Category), id);
        }
    }
}
