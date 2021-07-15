using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.Utilities.Filters;
using System.IO;
using System.Threading.Tasks;

namespace Northwind.Web.Controllers
{
    [LogAction(true)]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
            => View(await _categoryService.GetAllAsync());

        [Route("images/{id}")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> GetImage(int id)
            => File(await _categoryService.GetPictureByIdAsync(id), "image/bmp");

        [HttpGet]
        public async Task<IActionResult> EditImage(int id)
            => View(await _categoryService.GetByIdAsync(id));


        [HttpPost]
        public async Task<IActionResult> EditImage(CategoryDTO category, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                await using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                category.Picture = memoryStream.ToArray();

                await _categoryService.UpdateAsync(category);
            }

            return RedirectToAction("Index");
        }
    }
}
