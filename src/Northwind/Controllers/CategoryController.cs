using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Filter;
using Northwind.Models;
using Northwind.Repositories;
using System.IO;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [LogAction(true)]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _db;
        public CategoryController(IRepository<Category> db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var categories = _db.FindAll();
            return View(categories);
        }

        [Route("images/{id}")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> GetImage(int? id)
        {
            var category = await _db.FindAsync(id);

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
            var category = await _db.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(Category category, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                await using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                category.Picture = memoryStream.ToArray();

                _db.Update(category);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
