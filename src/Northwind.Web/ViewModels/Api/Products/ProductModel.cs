using Northwind.Web.ViewModels.Api.Categories;

namespace Northwind.Web.ViewModels.Api.Products
{
    public class ProductModel
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

        public SupplierModel Supplier { get; set; }

        public int CategoryID { get; set; }

        public CategoryModel Category { get; set; }

    }
}
