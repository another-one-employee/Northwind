using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Filter;
using Northwind.Models;
using Northwind.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [LogAction(true)]
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _db;
        private readonly IRepository<Supplier> _dbSupplier;
        private readonly IRepository<Category> _dbCategory;
        private readonly int _maximumAmmountOfProducts;

        public ProductController(
            IRepository<Product> db,
            IRepository<Supplier> dbSupplier,
            IRepository<Category> dbCategory,
            IConfiguration configuration)
        {
            _db = db;
            _dbSupplier = dbSupplier;
            _dbCategory = dbCategory;
            _maximumAmmountOfProducts =
                configuration.GetValue<int>("MaximumAmmountOfProducts");
        }

        public async Task<IActionResult> Index()
        {
            var products = _db.FindAll()
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
                await _db.InsertAsync(product);
                _db.SaveChanges();

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
                Product product = await _db.FindAsync(id);
                if (product != null)
                {
                    PopulateProductsDropDownLists();
                    return View(product);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Update(product);
                _db.SaveChanges();

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
            ViewBag.Suppliers = new SelectList(_dbSupplier.FindAll(), "SupplierID", "CompanyName");
            ViewBag.Categories = new SelectList(_dbCategory.FindAll(), "CategoryID", "CategoryName");
        }
    }
}
