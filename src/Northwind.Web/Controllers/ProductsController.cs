using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.Utilities.Filters;
using Northwind.Web.ViewModels.Products;
using System;
using System.Threading.Tasks;

namespace Northwind.Web.Controllers
{
    [LogAction(true)]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAsyncRepository<SupplierDTO> _dbSupplier;
        private readonly IAsyncRepository<CategoryDTO> _dbCategory;
        private readonly IMapper _mapper;
        private readonly Lazy<int> _lazyMaxAmountOfProducts;
        protected int MaxAmountOfProducts => _lazyMaxAmountOfProducts.Value;

        public ProductsController(
            IProductService productService,
            IAsyncRepository<SupplierDTO> dbSupplier,
            IAsyncRepository<CategoryDTO> dbCategory,
            IConfiguration configuration,
            IMapper mapper)
        {
            _productService = productService;
            _dbSupplier = dbSupplier;
            _dbCategory = dbCategory;
            _mapper = mapper;
            _lazyMaxAmountOfProducts = new Lazy<int>(
                configuration.GetValue<int>(nameof(MaxAmountOfProducts)));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetMaxAmountAsync(MaxAmountOfProducts));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateProductsDropDownLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateAsync(_mapper.Map<ProductDTO>(product));

                return RedirectToAction("Index");
            }
            else
            {
                await PopulateProductsDropDownLists();
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            await PopulateProductsDropDownLists();
            return View(_mapper.Map<EditProductViewModel>(await _productService.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(_mapper.Map<ProductDTO>(product));
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
            ViewBag.Suppliers = new SelectList(await _dbSupplier.FindAllAsync(), "SupplierID", "CompanyName");
            ViewBag.Categories = new SelectList(await _dbCategory.FindAllAsync(), "CategoryID", "CategoryName");
        }
    }
}
