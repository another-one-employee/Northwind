using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Exceptions;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.ViewModels.Api.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly Lazy<int> _lazyMaxAmountOfProducts;
        protected int MaxAmountOfProducts => _lazyMaxAmountOfProducts.Value;

        public ProductsController(IProductService productService, IConfiguration configuration, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
            _lazyMaxAmountOfProducts = new Lazy<int>(
                configuration.GetValue<int>(nameof(MaxAmountOfProducts)));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<ProductModel>>(await _productService.GetMaxAmountAsync(MaxAmountOfProducts)));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get products");
            }
        }
 
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(_mapper.Map<ProductModel>(await _productService.GetByIdAsync(id)));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get the product");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductModel>> Post(CreateProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _mapper.Map<ProductDTO>(model);
                    await _productService.CreateAsync(product);
                    return Created($"/api/products/{product.ProductID}", model);
                }

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create the product");
            }
        }

        [HttpPut]
        public async Task<ActionResult<UpdateProductModel>> Put(UpdateProductModel model)
        {
            try
            {
                if (ModelState.IsValid && model.ProductID != 0)
                {
                    var product = _mapper.Map<ProductDTO>(model);
                    await _productService.UpdateAsync(product);
                    return model;
                }

                return BadRequest();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the product");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                await _productService.DeleteAsync(product);
                return Ok(_mapper.Map<ProductModel>(product));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the product");
            }
        }
    }
}
