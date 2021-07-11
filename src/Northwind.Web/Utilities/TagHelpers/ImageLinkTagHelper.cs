using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Northwind.Web.Utilities.TagHelpers
{
    public class ImageLinkTagHelper : TagHelper
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("href", $"images/{Id}");
            output.Content.SetContent(Text);
        }
    }
}
