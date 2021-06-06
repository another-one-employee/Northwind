using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class HomeController : Controller
    {
        private readonly NorthwindDataContext _db;
        private readonly IConfiguration _configuration;

        public HomeController(NorthwindDataContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
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

        public IActionResult Products()
        {
            int amount = _configuration.GetValue<int>("MaximumAmmountOfProducts");

            var products =  _db.Products
                .Include(s => s.Supplier)
                .Include(c => c.Category)
                .ToList()
                .TakeWhile((p, i) => i < amount || amount == 0);

            return View(products);
        }
    }
}
