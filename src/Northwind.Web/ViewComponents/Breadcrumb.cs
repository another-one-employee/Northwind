using Microsoft.AspNetCore.Mvc;

namespace Northwind.Web.ViewComponents
{
    public class Breadcrumb : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var link = GetBreadcrumbItem();
            return View(link);
        }

        public BreadcrumbItem GetBreadcrumbItem()
        {
            var controller = ViewContext.RouteData.Values["controller"].ToString();
            var action = ViewContext.RouteData.Values["action"].ToString();

            return new BreadcrumbItem() { Action = action, Controller = controller };
        }
    }
}
