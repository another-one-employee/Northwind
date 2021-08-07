using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Application.Interfaces;
using Northwind.Web.ViewModels.Api.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.Application.Exceptions;
using Northwind.Domain.Entities;

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

        /// <summary>
        /// Get all Products
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productService.GetMaxAmountAsync(MaxAmountOfProducts);
                var productModels = _mapper.Map<IEnumerable<ProductModel>>(products);
                return Ok(productModels);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get products");
            }
        }

        /// <summary>
        /// Get a concrete Product
        /// </summary>
        /// <response code="404">If the item is null or was not found</response>    
        /// <response code="500">An error occurred on the server side</response>    
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                var productModel = _mapper.Map<ProductModel>(product);
                return Ok(productModel);
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

        /// <summary>
        /// Create a concrete Product
        /// </summary>
        /// <returns>A create Product</returns>
        /// <response code="201">Returns new item's values</response>
        /// <response code="400">If the validations failed</response>    
        /// <response code="500">An error occurred on the server side</response>  
        [HttpPost]
        public async Task<ActionResult<CreateProductModel>> Post(CreateProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _mapper.Map<ProductEntity>(model);
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

        /// <summary>
        /// Update a Product
        /// </summary>
        /// <returns>An updated Product</returns>
        /// <response code="400">If the validations failed</response>    
        /// <response code="404">If the item is null or was not found</response>   
        /// <response code="500">An error occurred on the server side</response>  
        [HttpPut]
        public async Task<ActionResult<UpdateProductModel>> Put(UpdateProductModel model)
        {
            try
            {
                if (ModelState.IsValid && model.ProductID != 0)
                {
                    var product = _mapper.Map<ProductEntity>(model);
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

        /// <summary>
        /// Delete a concrete Product
        /// </summary>
        /// <returns>A deleted Product</returns>
        /// <response code="404">If the item is null or was not found</response>    
        /// <response code="500">An error occurred on the server side</response>  
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                await _productService.DeleteAsync(product);
                var productModel = _mapper.Map<ProductModel>(product);
                return Ok(productModel);
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
