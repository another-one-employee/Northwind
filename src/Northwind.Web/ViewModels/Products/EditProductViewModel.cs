using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Products
{
    public class EditProductViewModel : CreateProductViewModel
    {
        [Key]
        public int ProductID { get; set; }
    }
}
