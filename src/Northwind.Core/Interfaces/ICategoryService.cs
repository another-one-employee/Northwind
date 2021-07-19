using Northwind.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByIdAsync(int id);
        Task<byte[]> GetPictureByIdAsync(int id);
        Task EditImageById(int id, byte[] image);
    }
}