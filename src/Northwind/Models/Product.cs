using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Supplier Supplier { get; set; }
        public Category Category { get; set; }
        public string QuantityPerUnit { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
