using Northwind.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 922337203685477.5807)]
        public decimal UnitPrice { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "The value must be positive")]
        public short UnitsInStock { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "The value must be positive")]
        public short UnitsOnOrder { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "The value must be positive")]
        public short ReorderLevel { get; set; }

        [Required]
        public bool Discontinued { get; set; }

        public int SupplierID { get; set; }
        public SupplierEntity SupplierEntity { get; set; }

        public int CategoryID { get; set; }
        public CategoryEntity CategoryEntity { get; set; }
    }
}
