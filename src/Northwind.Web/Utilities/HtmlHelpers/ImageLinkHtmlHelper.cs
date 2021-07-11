using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Encodings.Web;

namespace Northwind.Web.Utilities.HtmlHelpers
{
    public static class ImageLinkHtmlHelper
    {
        public static HtmlString ImageLink(this IHtmlHelper html, int imageId, string linkText)
        {
            using var writer = new System.IO.StringWriter();
            var url = new UrlHelper(html.ViewContext);

            TagBuilder a = new("a");
            a.InnerHtml.Append(linkText);
            a.MergeAttribute("href", $"images/{imageId}");

            a.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}
