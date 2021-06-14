using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Northwind.Data.Models;
using Northwind.Models.HomeViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class HomeController : Controller
    {
        private readonly NorthwindDataContext _db;
        private readonly ILogger<ErrorViewModel> _loggerOfErrors;

        public HomeController(
            NorthwindDataContext db,
            ILogger<ErrorViewModel> loggerOfErrors)
        {
            _db = db;
            _loggerOfErrors = loggerOfErrors;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel()
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

#nullable enable
            Exception? error = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
#nullable disable
            if (error != null)
            {
                errorModel.ExceptionMessage = error.Message;
                _loggerOfErrors.LogError(errorModel.ExceptionMessage);
            }

            return View(errorModel);
        }
    }
}
