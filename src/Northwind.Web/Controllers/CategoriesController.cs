using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Web.Utilities.Filters;
using Northwind.Web.ViewModels.Categories;
using System.IO;
using System.Threading.Tasks;
using Northwind.Application.Interfaces;

namespace Northwind.Web.Controllers
{
    [LogAction(true)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
            => View(await _categoryService.GetAllAsync());

        [Route("images/{id}")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> GetImage(int id)
            => File(await _categoryService.GetPictureByIdAsync(id), "image/bmp");

        [HttpGet]
        public async Task<IActionResult> EditImage(int id)
            => View(_mapper.Map<EditImageViewModel>(await _categoryService.GetByIdAsync(id)));


        [HttpPost]
        public async Task<IActionResult> EditImage(EditImageViewModel model, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                await using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                model.Picture = memoryStream.ToArray();

                await _categoryService.EditImageById(model.CategoryID, model.Picture);
            }

            return RedirectToAction("Index");
        }
    }
}
