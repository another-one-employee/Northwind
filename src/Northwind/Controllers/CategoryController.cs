using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data.Models;
using Northwind.Models;
using System.IO;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NorthwindDataContext _db;
        public CategoryController(NorthwindDataContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        [Route("images/{id}")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> GetImage(int? id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);

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
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);

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
                using (var memoryStream = new MemoryStream())
                {
                    await uploadedFile.CopyToAsync(memoryStream);
                    category.Picture = memoryStream.ToArray();
                }

                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
