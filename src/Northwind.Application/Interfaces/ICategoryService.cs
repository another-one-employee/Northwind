using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.Domain.Entities;

namespace Northwind.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryEntity>> GetAllAsync();
        Task<CategoryEntity> GetByIdAsync(int id);
        Task<byte[]> GetPictureByIdAsync(int id);
        Task EditImageById(int id, byte[] image);
    }
}