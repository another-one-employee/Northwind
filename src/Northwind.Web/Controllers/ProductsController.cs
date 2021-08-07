using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Northwind.Web.Utilities.Filters;
using Northwind.Web.ViewModels.Products;
using System;
using System.Threading.Tasks;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;

namespace Northwind.Web.Controllers
{
    [LogAction(true)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly Lazy<int> _lazyMaxAmountOfProducts;
        protected int MaxAmountOfProducts => _lazyMaxAmountOfProducts.Value;

        public ProductsController(
            IProductService productService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _productService = productService;
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
                await _productService.CreateAsync(_mapper.Map<ProductEntity>(product));

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
                await _productService.UpdateAsync(_mapper.Map<ProductEntity>(product));
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
            ViewBag.Suppliers = new SelectList(await _productService.GetSuppliers(), "SupplierID", "CompanyName");
            ViewBag.Categories = new SelectList(await _productService.GetCategories(), "CategoryID", "CategoryName");
        }
    }
}
