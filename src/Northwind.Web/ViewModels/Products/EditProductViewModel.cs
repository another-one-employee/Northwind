using Northwind.Core.Models;

namespace Northwind.Web.ViewModels.Products
{
    public class EditProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }

        public short ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public int SupplierID { get; set; }
        public SupplierDTO Supplier { get; set; }

        public int CategoryID { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
