using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<byte[]> GetPictureByIdAsync(int id);
        Task EditImageById(int id, byte[] image);
    }
}