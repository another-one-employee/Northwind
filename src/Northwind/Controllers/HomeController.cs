using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Products()
        {
            int amount = _configuration.GetValue<int>("MaximumAmmountOfProducts");
            var products = _db.Products
                .Include(s => s.Supplier)
                .Include(c => c.Category);

            if (amount == 0)
            {
                return View(await products.ToListAsync());
            }

            return View(await products.Take(amount).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Suppliers = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Add(product);
                await _db.SaveChangesAsync();

                return RedirectToAction("Products");
            }
            else
            {
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Product product = await _db.Products.FirstOrDefaultAsync(p => p.ProductID == id);
                if (product != null)
                {
                    ViewBag.Suppliers = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
                    ViewBag.Categories = new SelectList(_db.Categories, "CategoryID", "CategoryName");
                    return View(product);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(product);
                await _db.SaveChangesAsync();

                return RedirectToAction("Products");
            }
            else
            {
                return View(product);
            }
        }
    }
}
