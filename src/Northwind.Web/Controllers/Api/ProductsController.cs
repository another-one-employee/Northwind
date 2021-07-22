using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Northwind.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly Lazy<int> _lazyMaxAmountOfProducts;
        protected int MaxAmountOfProducts => _lazyMaxAmountOfProducts.Value;

        public ProductsController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _lazyMaxAmountOfProducts = new Lazy<int>(
                configuration.GetValue<int>(nameof(MaxAmountOfProducts)));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _productService.GetMaxAmountAsync(1)); // todo: return maxAmount
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get products");
            }
        }
    }
}
