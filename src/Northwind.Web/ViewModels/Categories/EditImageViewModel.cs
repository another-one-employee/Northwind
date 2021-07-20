using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Categories
{
    public class EditImageViewModel
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public byte[] Picture { get; set; }
    }
}
