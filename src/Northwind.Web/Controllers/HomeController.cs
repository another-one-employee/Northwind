using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Northwind.Web.Filters;
using Northwind.Web.ViewModels.Home;
using System;
using System.Diagnostics;

namespace Northwind.Web.Controllers
{
    [LogAction]
    public class HomeController : Controller
    {
        private readonly ILogger<ErrorViewModel> _loggerOfErrors;

        public HomeController(ILogger<ErrorViewModel> loggerOfErrors)
        {
            _loggerOfErrors = loggerOfErrors;
        }

        public IActionResult Index()
        {
            return View();
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
