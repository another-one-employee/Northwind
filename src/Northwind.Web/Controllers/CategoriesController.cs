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
        private readonly IRepository<CategoryDTO> _db;
        public CategoriesController(IRepository<CategoryDTO> db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _db.FindAllAync();
            return View(categories);
        }

        [Route("images/{id}")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> GetImage(int? id)
        {
            var category = await _db.FindAync(id);

            if (category == null)
            {
                return NotFound();
            }

            var image = category.Picture;
            return File(image, "image/bmp");
        }

        [HttpGet]
        public async Task<IActionResult> EditImage(int? id)
        {
            var category = await _db.FindAync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(CategoryDTO category, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                await using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                category.Picture = memoryStream.ToArray();

                _db.Update(category);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
