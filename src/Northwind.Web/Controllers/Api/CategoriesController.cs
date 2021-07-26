using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Exceptions;
using Northwind.Core.Interfaces;
using Northwind.Web.ViewModels.Api.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Categories
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<CategoryModel>>(await _categoryService.GetAllAsync()));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get categories");
            }
        }

        /// <summary>
        /// Get a concrete category image
        /// </summary>
        /// <returns>A CategoryImageModel</returns>
        /// <response code="404">If the item is null or was not found</response>    
        /// <response code="500">An error occurred on the server side</response>  
        [HttpGet("image/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var picture = await _categoryService.GetPictureByIdAsync(id);
                return Ok(new CategoryImageModel { CategoryID = id, Picture = picture });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get the category image");
            }
        }

        /// <summary>
        /// Update a concrete category image
        /// </summary>
        /// <returns>A CategoryImageModel</returns>
        /// <response code="400">If the validations failed</response>    
        /// <response code="404">If the item is null or was not found</response>   
        /// <response code="500">An error occurred on the server side</response>  
        [HttpPut("image")]
        public async Task<ActionResult<CategoryImageModel>> Put(CategoryImageModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryService.EditImageById(model.CategoryID, model.Picture);
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the category image");
            }
        }
    }
}
