using AutoMapper;
using Northwind.Core.Exceptions;
using Northwind.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Category = Northwind.Core.Entities.Category;

namespace Northwind.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IAsyncRepository<Models.Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IAsyncRepository<Models.Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var models = await _categoryRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<Category>>(models);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var model = await FindByIdAsync(id);
            return _mapper.Map<Category>(model);
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

        private async Task<Models.Category> FindByIdAsync(int id)
        {
            return await _categoryRepository.FindAsync(c => c.CategoryId == id) ??
                   throw new NotFoundException(nameof(Category), id);
        }
    }
}
