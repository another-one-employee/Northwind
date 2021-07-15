using Northwind.Core.Exceptions;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IAsyncRepository<CategoryDTO> _categoryRepository;

        public CategoryService(IAsyncRepository<CategoryDTO> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            return await _categoryRepository.FindAllAsync();
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            return await _categoryRepository.FindAsync(id) ?? throw new NotFoundException(nameof(CategoryDTO), id);
        }

        public async Task<byte[]> GetPictureByIdAsync(int id)
        {
            var category = await GetByIdAsync(id);

            return category.Picture;
        }

        public async Task UpdateAsync(CategoryDTO category)
        {
            await _categoryRepository.Update(category);
        }
    }
}
