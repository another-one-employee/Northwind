using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Data.Models;
using Northwind.Filter;
using Northwind.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [LogAction(true)]
    public class ProductController : Controller
    {
        private readonly NorthwindDataContext _db;
        private readonly int _maximumAmmountOfProducts;

        public ProductController(
            NorthwindDataContext db,
            IConfiguration configuration)
        {
            _db = db;
            _maximumAmmountOfProducts =
                configuration.GetValue<int>("MaximumAmmountOfProducts");
        }

        public async Task<IActionResult> Index()
        {
            var products = _db.Products
                .Include(s => s.Supplier)
                .Include(c => c.Category);

            if (_maximumAmmountOfProducts == 0)
            {
                return View(await products.ToListAsync());
            }

            return View(await products.Take(_maximumAmmountOfProducts).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateProductsDropDownLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Add(product);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                PopulateProductsDropDownLists();
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
                    PopulateProductsDropDownLists();
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

                return RedirectToAction("Index");
            }
            else
            {
                PopulateProductsDropDownLists();
                return View(product);
            }
        }

        private void PopulateProductsDropDownLists()
        {
            ViewBag.Suppliers = new SelectList(_db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryID", "CategoryName");
        }
    }
}
