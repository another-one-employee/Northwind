using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Northwind.Web.Utilities.Filters
{
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private readonly bool _logParams;
        public LogActionFilter(ILogger<LogActionFilter> logger, bool logParams = false)
        {
            _logParams = logParams;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("--STARTING--" + context.ActionDescriptor.DisplayName);

            if (_logParams)
            {
                foreach (var p in context.ActionArguments)
                {
                    _logger.LogInformation($"{p.Key} : {p.Value}");
                }
            }

            await next();

            _logger.LogInformation("--ENDING--" + context.ActionDescriptor.DisplayName);
        }
    }
}
