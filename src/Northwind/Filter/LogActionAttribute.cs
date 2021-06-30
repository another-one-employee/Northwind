using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Northwind.Filter
{
    public class LogActionAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        private readonly bool _logParams;

        public LogActionAttribute(bool logParams = false)
        {
            _logParams = logParams;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var logger = (ILogger<LogActionFilter>)serviceProvider.GetService(typeof(ILogger<LogActionFilter>));
            return new LogActionFilter(logger, _logParams);
        }
    }
}
