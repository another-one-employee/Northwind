using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Core.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Range(0, 9999.99)]
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [Range(0, 32767)]
        public short UnitsInStock { get; set; }

        [Range(0, 32767)]
        public short UnitsOnOrder { get; set; }

        [Range(0, 32767)]
        public short ReorderLevel { get; set; }

        [Required]
        public bool Discontinued { get; set; }

        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
