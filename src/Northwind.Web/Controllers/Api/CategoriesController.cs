using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Exceptions;
using Northwind.Core.Interfaces;
using Northwind.Web.ViewModels.Api.Categories;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(_mapper.Map<CategoryModel>(await _categoryService.GetAllAsync()));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get categories");
            }
        }

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
