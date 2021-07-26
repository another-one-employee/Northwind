using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Api.Categories
{
    public class CategoryImageModel
    {
        [Required]
        public int CategoryID { get; set; }
        public byte[] Picture { get; set; }
    }
}
