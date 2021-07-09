using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.Utilities.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Controllers
{
    [LogAction(true)]
    public class ProductsController : Controller
    {
        private readonly IRepository<ProductDTO> _db;
        private readonly IRepository<SupplierDTO> _dbSupplier;
        private readonly IRepository<CategoryDTO> _dbCategory;
        private readonly int _maximumAmountOfProducts;
        private readonly string _maximumAmountOfProductsKey = "MaximumAmountOfProducts";

        public ProductsController(
            IRepository<ProductDTO> db,
            IRepository<SupplierDTO> dbSupplier,
            IRepository<CategoryDTO> dbCategory,
            IConfiguration configuration)
        {
            _db = db;
            _dbSupplier = dbSupplier;
            _dbCategory = dbCategory;
            _maximumAmountOfProducts =
                configuration.GetValue<int>(_maximumAmountOfProductsKey);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _db.FindAllAync();

            if (_maximumAmountOfProducts == 0)
            {
                return View(products);
            }

            return View(products.Take(_maximumAmountOfProducts));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateProductsDropDownLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                await _db.InsertAsync(product);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                await PopulateProductsDropDownLists();
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                ProductDTO product = await _db.FindAync(id);
                if (product != null)
                {
                    await PopulateProductsDropDownLists();
                    return View(product);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                _db.Update(product);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                await PopulateProductsDropDownLists();
                return View(product);
            }
        }

        private async Task PopulateProductsDropDownLists()
        {
            ViewBag.Suppliers = new SelectList(await _dbSupplier.FindAllAync(), "SupplierID", "CompanyName");
            ViewBag.Categories = new SelectList(await _dbCategory.FindAllAync(), "CategoryID", "CategoryName");
        }
    }
}
