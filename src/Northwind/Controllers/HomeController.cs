using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class HomeController : Controller
    {
        private readonly NorthwindDataContext _db;

        public HomeController(NorthwindDataContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Products()
        {
            var products = await _db.Products
                .Include(s => s.Supplier)
                .Include(c => c.Category)
                .ToListAsync();

            return View(products);
        }
    }
}
