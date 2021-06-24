using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data.Models;
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
    }
}
